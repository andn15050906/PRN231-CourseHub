export { VideoHandler }

class VideoHandler {
    #videoContainer;
    #video;
    #stream;

    #isOn;
    isOnVideo;
    #isOnAudio;

    constructor(videoContainer, participantId) {
        this.#videoContainer = videoContainer;
        this.#video = videoContainer.querySelector(`#video_${participantId}`);
    }

    turnOn(stream) {
        if (!this.#isOn) {
            console.log("turning on video...");
            this.#isOn = true;
            this.turnOnVideo(stream);
        }
    }

    turnOff() {
        if (this.#isOn) {
            //var cachedStream = _stream.clone();
            this.turnOffVideo();
            this.mute();
            this.#isOn = false;

            //this.#video.srcObject = cachedStream;
        }
    }

    dispose() {
        this.#stream.getVideoTracks().forEach((track) => track.stop());
        this.#videoContainer.parentElement.removeChild(this.#videoContainer);
    }






    turnOnVideo(stream) {
        this.#stream = stream;
        this.#video.srcObject = stream;
        this.isOnVideo = true;
    }

    turnOffVideo() {
        this.#stream.getVideoTracks().forEach((track) => track.stop());
        this.#video.srcObject = null;
        this.isOnVideo = false;
    }

    //unmute

    mute() {
        this.#stream.getAudioTracks().forEach((track) => track.stop());
    }
}