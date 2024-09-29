import { HubConnection, RTCHandler, STREAM_EVENTS, ParticipantExtraInfo } from '../apis/hubGateway.js';
import {
    createSidebar, createInCallWindow, createOutCallWindow, createVideoTile,
    createMessageContainer, createMessage
} from './call_html.js';
import { VideoHandler } from '../chat/videoHandler.js';
import { ChatHandler, ChatMessage } from '../chat/chatHandler.js';
import * as Storage from '../common/storage.js';
import { redirectToSignin } from '../common/utilities.js';






const MEDIA_TYPE = {
    DISPLAY_MEDIA: "DISPLAY_MEDIA",
    USER_MEDIA: "USER_MEDIA"
};
const constraints = {
    audio: { echoCancellation: true },
    video: { width: 400, height: 300 }
};



var client = Storage.getClientData();
if (!client) {
    if (window.unsetClient) {
        Storage.setClientData(window.unsetClient);
    }
    else {
        redirectToSignin();
    }
}

var _roomId = 'eb4e0163-271a-4886-9e60-bee318d7ca44';                //storage
//...
var conversationName = window.conversationName;
_roomId = window.conversationId;
/*if (!conversationName)
    conversationName = "Video call";*/
var _onCall = false;

var callWindow;
var _videoRow, roomChatInp, _defaultMedia = MEDIA_TYPE.DISPLAY_MEDIA;
//var sideBarPagination = 1;

var _rtcHandler = new RTCHandler(new HubConnection());
var _participants = {};
var _localStream;

setupRTCHandler(_rtcHandler);
callWindow = document.getElementById('_callWindow');



//document.getElementById('_sidebar').innerHTML = createSidebar();



setCallWindow(_onCall);
var _chatHandler;



function setCallWindow(isOnCall) {
    _onCall = isOnCall;

    if (!_onCall) {
        callWindow.innerHTML = createOutCallWindow(conversationName);
        document.getElementById('_join-room-btn').onclick = () => {
            if (isValidRoomId(_roomId)) {
                _rtcHandler.joinRoom(_roomId, new ParticipantExtraInfo(client.fullName, client.avatar));
            }
        }
    }
    else {
        callWindow.innerHTML = createInCallWindow();

        _videoRow = document.getElementById('_video-row');

        document.getElementById('_camera-btn').onclick = () => switchMedia(MEDIA_TYPE.USER_MEDIA);
        document.getElementById('_share-screen-btn').onclick = () => switchMedia(MEDIA_TYPE.DISPLAY_MEDIA);
        document.getElementById('_hang-up-btn').onclick = hangUp;
        document.getElementById('_bottom-controls').style.opacity = 1;


        /*document.getElementById('_message-layer-container').innerHTML = createMessageContainer();
        document.getElementById('_close-message-container-btn').onclick = () => {
            document.getElementById('_message-layer-container').style.display = 'none';
        }
        roomChatInp = document.getElementById('_message-inp');
        roomChatInp.onkeydown = (e) => {
            if (e.key === 'Enter') {
                e.preventDefault();

                let message = new ChatMessage(roomChatInp.value, null);
                message.setCreationData(client.fullName);
                _rtcHandler.sendRoomChatMessage(_roomId, JSON.stringify(message));
                _chatHandler.addMessage(message);
                roomChatInp.value = "";
            }
        }
        _chatHandler = new ChatHandler(document.getElementById('_message_lst'), createMessage, []);*/

        /*_chatHandler.addMessage("");
        _chatHandler.addMessage("");*/
    }
}

function setupRTCHandler(rtcHandler) {
    rtcHandler.onRoomCreated = async (roomId) => {
        addRoomUI([]);
        await tryGetMedia(_defaultMedia);
    }
    rtcHandler.onRoomInfoReceived = async (roomInfo) => {
        if (roomInfo.id == _roomId) {
            let peers = [];
            let peerIds = [];
            let clientConnection = _rtcHandler.getConnectionId();
            for (let participant of roomInfo.participants) {
                if (participant.connectionId != clientConnection) {
                    peers.push(participant);
                    peerIds.push(participant.connectionId);
                }
            }

            setupConnections(peerIds);
            addRoomUI(peers);
            await tryGetMedia(_defaultMedia);
            if (_localStream != null)
                rtcHandler.setupRTCStream(peerIds, _localStream);
            else
                rtcHandler.setupRTCTransceiver(peerIds);
            rtcHandler.createAndSendOffers(peerIds, false);
        }
    }
    rtcHandler.onRoomJoined = (roomId, participant) => {
        if (_roomId == roomId) {
            addParticipantHandler(participant);
        }
    }
    rtcHandler.onOfferReceived = (peer, offer, isRenegotiation) => {
        // doesn't override existing connections & behaviors (if there are)
        if (!isRenegotiation) {
            setupConnections([peer]);
            if (_localStream != null)
                rtcHandler.setupRTCStream([peer], _localStream);
        }
        rtcHandler.answer(peer, JSON.parse(offer));
    }
    rtcHandler.onAnswerReceived = (answerer, answer) => {
        rtcHandler.setRemoteDescription(answerer, JSON.parse(answer));
    }
    rtcHandler.onICECandidateReceived = (peer, candidate) => {
        rtcHandler.addIceCandidate(peer, candidate);
    }
    rtcHandler.onRoomMessageReceived = (roomId, peerId, content) => {
        // message: hubGateway.StreamMessage

        if (roomId == _roomId) {
            switch (content.event) {
                case STREAM_EVENTS.VideoOff:
                    _participants[peerId].videoHandler.turnOff();
                    break;
                case STREAM_EVENTS.RoomMessageReceived:
                    _chatHandler.addMessage(JSON.parse(content.data));
                    break;
            }
        }
    }
    rtcHandler.onParticipantLeft = (roomId, peer) => {
        console.log(peer + " left " + roomId);
        rtcHandler.removeConnection(peer);
        _participants[peer].dispose();
    }



    function addRoomUI(peers) {
        setCallWindow(true);
        client.connectionId = _rtcHandler.getConnectionId();
        addParticipantHandler(client);
        for (let peer of peers)
            addParticipantHandler(peer);
    }
    async function tryGetMedia(mediaType) {
        try {
            _localStream = await getMediaStream(mediaType);
            _participants[client.connectionId].videoHandler.turnOn(_localStream);
        }
        catch (e) {
            //denied
        }
    }
    function setupConnections(peers) {
        rtcHandler.setupRTCPeerConnections(peers);
        rtcHandler.onRTCTrack = (peer, trackEvent) => {
            // stopping track only fires mute if the connection is alive
            // replaceTrack doesn't fire ontrack, onmute or onunmute

            console.log("New ontrack");
            _participants[peer].videoHandler.turnOn(trackEvent.streams[0]);
        }
    }
    function addParticipantHandler(participant) {
        let videoContainer = createVideoTile(participant.connectionId);
        _videoRow.appendChild(videoContainer);
        _participants[participant.connectionId] = new ParticipantHandler(participant);
        _participants[participant.connectionId].videoHandler = new VideoHandler(videoContainer, participant.connectionId);
    }
}






async function getMediaStream(mediaType) {
    switch (mediaType) {
        case MEDIA_TYPE.DISPLAY_MEDIA:
            return await navigator.mediaDevices.getDisplayMedia();
        case MEDIA_TYPE.USER_MEDIA:
            return await navigator.mediaDevices.getUserMedia(constraints);
    }
    return null;
}

async function switchMedia(mediaType) {
    if (client.connectionId != null) {
        let videoHandler = _participants[client.connectionId].videoHandler;
        if (!videoHandler.isOnVideo) {
            let newStream = await getMediaStream(mediaType);
            if (newStream != null) {
                videoHandler.turnOnVideo(newStream);
                _rtcHandler.renegotiate(getPeerIds(), newStream);
            }
        }
        else {
            videoHandler.turnOffVideo();
            _rtcHandler.sendRoomVideoOff(_roomId);
        }
    }
}

function hangUp() {
    _participants[client.connectionId].videoHandler.turnOff();
    _rtcHandler.leaveRoom(_roomId);
    setCallWindow(false);
    //sideBarPagination = 1;
    //_roomId = '';
    //messages = [];
}

function isValidRoomId(roomId) {
    return typeof roomId == "string" && _roomId.length > 0;
}

function getPeerIds() {
    return Object.keys(_participants).filter(connectionId => connectionId != _rtcHandler.getConnectionId());
}

class ParticipantHandler {
    participant;
    videoHandler;

    constructor(participant) {
        this.participant = participant;
    }

    dispose() {
        if (this.videoHandler != null) {
            this.videoHandler.dispose();
        }
    }
}