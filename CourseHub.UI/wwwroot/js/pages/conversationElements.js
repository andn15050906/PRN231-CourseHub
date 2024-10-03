import { createDiv } from "../common/utilities.js";
import { getClientData, getUserInfo } from '../common/storage.js';

export { createMessage }





// used as a callback for state handler
function createMessage(message) {
    var client = getClientData();

    var isClientMessage = client.id == message.creatorId;
    var sender = getUserInfo(message.creatorId);
    var senderAvatar = isClientMessage ? client.avatarUrl : sender.avatarUrl;
    var senderName = isClientMessage ? client.fullName : sender.fullName;

    if (!message.lastModificationTime)
        message.lastModificationTime = new Date().toLocaleString();

    /*onclick="openDelete('${message.id}')"*/
    if (!isClientMessage) {
        return createDiv(
            `<!-- Message -->
                <!-- Avatar -->
                <a class="avatar avatar-sm mr-4 mr-lg-5" href="#" data-chat-sidebar-toggle="#chat-2-info">
                    <img class="avatar-img" src="${senderAvatar}" alt="">
                </a>

                <!-- Message: body -->
                <div class="message-body">

                    <!-- Message: row -->
                    <div class="message-row">
                        <div class="d-flex align-items-center">
                            <!-- Message: content -->
                            <div class="message-content bg-light">
                                <h6 class="mb-2">${senderName}</h6>
                                <div>${message.content}</div>

                                <div class="mt-1">
                                    <small class="opacity-65">${message.lastModificationTime}</small>
                                </div>
                            </div>
                            <!-- Message: content -->
                        </div>
                    </div>
                    <!-- Message: row -->

                </div>
                <!-- Message: Body -->
            <!-- Message -->`, 'message', `app-msg-${message.id}`
        );
    }

    return createDiv(
        `<!-- Message -->
            <div class="avatar avatar-sm ml-4 ml-lg-5 d-none d-lg-block">
                    <img class="avatar-img" src="${client.avatarUrl}" alt="">
            </div>
            <div class="message-body">
                <div class="message-row">
                    <div class="d-flex align-items-center justify-content-end">
                        <div class="dropdown">
                            <a class="text-muted opacity-60 mr-3" href="#" onclick="openDelete('${message.id}')"
                                data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <i class="fe-more-vertical"></i>
                            </a>
                            <div class="dropdown-menu">
                                <a class="dropdown-item d-flex align-items-center" href="#">
                                    Edit <span class="ml-auto fe-edit-3"></span>
                                </a>
                                <a class="dropdown-item d-flex align-items-center" href="#">
                                    Share <span class="ml-auto fe-share-2"></span>
                                </a>
                                <a class="dropdown-item d-flex align-items-center" href="#">
                                    Delete <span class="ml-auto fe-trash-2"></span>
                                </a>
                            </div>
                        </div>
                        <div class="message-content bg-primary text-white">
                            <div>${message.content}</div>

                            <!--<div class="mt-1">
                                <small class="opacity-65">8 mins ago</small>
                            </div>-->

                            <div class="mt-1">
                                <small class="opacity-65">${message.lastModificationTime}</small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>`, 'message message-right', `app-msg-${message.id}`
    );
}