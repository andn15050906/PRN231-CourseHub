export {
    setClientData, getClientData,
    setUserInfo, getUserInfo,
    setToLocalStorage, getFromLocalStorageAsJson,
    getCookieByName
}



function getCookieByName(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}



function setClientData(clientModel) {
    setToLocalStorage('clientData', JSON.stringify(clientModel));
}

function getClientData() {
    return getFromLocalStorageAsJson('clientData');
}

function setUserInfo(id, model) {
    setToLocalStorage(`user_${id}`, JSON.stringify(model));
}

function getUserInfo(id) {
    return getFromLocalStorageAsJson(`user_${id}`);
}










function setToLocalStorage(name, value) {
    localStorage.setItem(name, value);
}

function getFromLocalStorageAsJson(name) {
    let obj = localStorage.getItem(name);
    if (obj)
        return JSON.parse(obj);
    return null;
}

function removeFromLocalStorage(name) {
    localStorage.removeItem(name);
}