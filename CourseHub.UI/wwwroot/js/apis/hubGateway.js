import { hubUrl } from '../constants.js';
import { getCookieByName } from '../common/storage.js';
export {
    HubConnection, MessagingHandler, RTCHandler,
    MESSAGE_TYPES, STREAM_EVENTS, ParticipantExtraInfo
}

// from <script>
var _signalR = signalR;



class HubConnection {
    #connection;

    constructor() {
        var bearer = getCookieByName('Bearer');
        console.log(bearer);

        this.#connection = new _signalR.HubConnectionBuilder().withUrl(hubUrl,/*
            { headers: { "ngrok-skip-browser-warning": "69420" } }
        */
            { accessTokenFactory: () => bearer }
        ).build();
    }

    subscribe(eventName, callback) {
        this.#connection.on(eventName, callback);
    }

    start() {
        this.#connection.start();
    }

    getConnectionId() {
        window.con = this.#connection.connection;
        return this.#connection.connection.connectionId;
    }






    // Messaging
    sendToConversation(message) {
        this.#connection.invoke("SendToConversation", message);
    }






    // Stream

    sendOffers(offerMessages, isRenegotiation) {
        this.#connection.invoke("SendOffers", offerMessages, isRenegotiation);
    }
    sendAnswer(offerer, answer) {
        this.#connection.invoke("SendAnswer", offerer, JSON.stringify(answer));
    }
    sendICECandidate(receiver, candidate) {
        this.#connection.invoke("SendICECandidate", receiver, JSON.stringify(candidate));
    }

    joinRoom(roomId, info) {
        this.#connection.invoke("JoinRoom", roomId, info);
    }
    sendRoom(roomId, message) {
        this.#connection.invoke("SendRoom", roomId, message);
    }
    leaveRoom(roomId) {
        this.#connection.invoke("LeaveRoom", roomId);
    }
}






class MessagingHandler {
    #hubConnection;

    constructor(hubConnection) {
        this.#hubConnection = hubConnection;



        this.#hubConnection.subscribe(MESSAGE_TYPES.ChatMessageCreated.Name, (conversationId, messageModel) => {
            this.onChatMessageCreated(conversationId, messageModel);
        })



        this.#hubConnection.start();
    }


    // these methods (events) should be overriden
    onChatMessageCreated = (conversationId, messageModel) => { }



    createChatMessage(conversationId, content, attachments) {
        var message = {
            receiverId: conversationId,
            type: attachments == null
                ? MESSAGE_TYPES.CreateTextChatMessage.Enum
                : MESSAGE_TYPES.CreateFileChatMessage.Enum,
            callback: MESSAGE_TYPES.ChatMessageCreated.Name,
            data: attachments == null
                ? content
                : JSON.stringify(attachments),
            time: new Date()
        }
        console.log(message);
        this.#hubConnection.sendToConversation(message);
    }
}






class RTCHandler {
    #hubConnection;
    #rtcPeerConnections = {};           // by connectionId

    //firefox suggest use 2
    #rtcConfiguration = {
        iceServers: [
            { urls: 'stun:stun.l.google.com:19302' },
            { urls: 'stun:stun1.l.google.com:19302' }
        ]
    }



    constructor(hubConnection) {
        this.#hubConnection = hubConnection;



        this.#hubConnection.subscribe(STREAM_EVENTS.OfferReceived, (offerer, offer, isRenegotiation) => {
            this.onOfferReceived(offerer, offer, isRenegotiation);
        })
        this.#hubConnection.subscribe(STREAM_EVENTS.AnswerReceived, (answerer, answer) => {
            this.onAnswerReceived(answerer, answer);
        });
        this.#hubConnection.subscribe(STREAM_EVENTS.ICECandidateReceived, (peer, candidate) => {
            this.onICECandidateReceived(peer, candidate);
        });



        this.#hubConnection.subscribe(STREAM_EVENTS.RoomCreated, (roomId) => {
            this.onRoomCreated(roomId);
        });
        this.#hubConnection.subscribe(STREAM_EVENTS.RoomInfoReceived, (roomInfo) => {
            this.onRoomInfoReceived(roomInfo);
        })
        this.#hubConnection.subscribe(STREAM_EVENTS.RoomJoined, (roomId, participant) => {
            this.onRoomJoined(roomId, participant);
        });
        this.#hubConnection.subscribe(STREAM_EVENTS.ParticipantLeft, (roomId, connectionId) => {
            this.onParticipantLeft(roomId, connectionId);
        });
        this.#hubConnection.subscribe(STREAM_EVENTS.RoomMessageReceived, (roomId, connectionId, message) => {
            this.onRoomMessageReceived(roomId, connectionId, message);
        });



        window.onbeforeunload = () => {
            for (let peer in this.#rtcPeerConnections)
                this.#rtcPeerConnections[peer].close();
        }
        this.#hubConnection.start();
    }

    // these methods (events) should be overriden
    onConversationMessageReceived = (conversationId, connectionId, message) => { }

    onOfferReceived = (offerer, offer, isRenegotiation) => { }
    onAnswerReceived = (answerer, answer) => { }
    onICECandidateReceived = (peer, candidate) => { }
    onRoomCreated = (roomId) => { }
    onRoomInfoReceived = (roomInfo) => { }
    onRoomJoined = (roomId, participant) => { }
    onRoomMessageReceived = (roomId, connectionId, message) => { }
    onParticipantLeft = (roomId, connectionId) => { }



    setupRTCPeerConnection = (peer) => {
        let connection = new RTCPeerConnection(this.#rtcConfiguration);

        connection.ontrack = (trackEvent) => {
            this.onRTCTrack(peer, trackEvent);
        }
        connection.onicecandidate = (event) => {
            if (event.candidate) {
                console.log("new candidate");
                this.#hubConnection.sendICECandidate(peer, event.candidate);
            }
        }

        this.#rtcPeerConnections[peer] = connection;
    }
    setupRTCPeerConnections = (peers) => {
        for (let peer of peers)
            this.setupRTCPeerConnection(peer);
    }
    setupRTCStream = (peers, stream) => {
        stream.getTracks().forEach((track) => {
            for (let peer of peers)
                this.#rtcPeerConnections[peer].addTrack(track, stream);
        });
    }
    setupRTCTransceiver = (peers) => {
        for (let peer of peers)
            this.#rtcPeerConnections[peer].addTransceiver('audio', { direction: 'recvonly' });
    }
    createAndSendOffers = async (peers, isRenegotiation) => {
        // offers are sent from the new participant
        let offerMessages = [];

        for (let peer of peers) {
            let connection = this.#rtcPeerConnections[peer];
            let offer = await connection.createOffer();
            connection.setLocalDescription(offer);
            offerMessages.push(new OfferMessage(JSON.stringify(offer), peer));
        }
        this.#hubConnection.sendOffers(offerMessages, isRenegotiation);
    }
    answer = async (peer, offer) => {
        this.setRemoteDescription(peer, offer);
        let connection = this.#rtcPeerConnections[peer];
        let sdpAnswer = await connection.createAnswer();
        connection.setLocalDescription(sdpAnswer);
        this.#hubConnection.sendAnswer(peer, sdpAnswer);
    }
    setRemoteDescription = (peer, description) => {
        this.#rtcPeerConnections[peer].setRemoteDescription(new RTCSessionDescription(description));
    }
    addIceCandidate = (peer, candidate) => {
        this.#rtcPeerConnections[peer].addIceCandidate(new RTCIceCandidate(JSON.parse(candidate)));
    }
    renegotiate = (peers, newStream) => {
        this.setupRTCStream(peers, newStream);
        this.createAndSendOffers(peers, true);
    }
    // Override this
    onRTCTrack = (peerId, trackEvent) => { }



    joinRoom = (roomId, info) => {
        this.#hubConnection.joinRoom(roomId, info);
    }
    leaveRoom = (roomId) => {
        this.#hubConnection.leaveRoom(roomId);
    }



    sendRoomChatMessage = (roomId, message) => {
        // message: chatHandler.ChatMessage
        this.#hubConnection.sendRoom(roomId, new StreamMessage(STREAM_EVENTS.RoomMessageReceived, message));
    }
    sendRoomVideoOff = (roomId) => {
        this.#hubConnection.sendRoom(roomId, new StreamMessage(STREAM_EVENTS.VideoOff));
    }



    getConnectionId = () => {
        return this.#hubConnection.getConnectionId();
    }
    removeConnection = (peer) => {
        delete this.#rtcPeerConnections[peer];
    }
}






const MESSAGE_TYPES = {
    CreateConversation: { Name: "CreateConversation", Enum: 0 },
    UpdateConversation: { Name: "UpdateConversation", Enum: 1 },

    CreateTextChatMessage: { Name: "CreateTextChatMessage", Enum: 2 },
    CreateFileChatMessage: { Name: "CreateFileChatMessage", Enum: 3 },

    ChatMessageCreated: { Name: "ChatMessageCreated", Enum: 4 },
}

const STREAM_EVENTS = {
    RoomCreated: "RoomCreated",
    RoomInfoReceived: "RoomInfoReceived",
    RoomJoined: "RoomJoined",
    ParticipantLeft: "ParticipantLeft",

    OfferReceived: "OfferReceived",
    AnswerReceived: "AnswerReceived",
    ICECandidateReceived: "ICECandidateReceived",

    RoomMessageReceived: "RoomMessageReceived",
    VideoOff: "VideoOff"
}

class ParticipantExtraInfo {
    constructor(fullName, avatar) {
        this.fullName = fullName;
        this.avatar = avatar;
    }
}

class StreamMessage {
    constructor(event, data) {
        this.event = event;
        this.data = data;
    }
}

class OfferMessage {
    constructor(offer, peer) {
        this.offer = offer;
        this.peer = peer;
    }
}