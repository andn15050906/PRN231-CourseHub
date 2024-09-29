export {
    getNonParsed, getParsedJson,
    postWithCredentials,
    postJsonWithCredentials,
    patchJsonWithCredentials, patchFormDataWithCredentials,
    deleteWithCredentials
}

import { getCookieByName } from '../common/storage.js';






async function getNonParsed(url) {
    return fetch(url);
}

async function getParsedJson(url) {
    return fetch(url)
        .then(res => res.json())
        .catch(error => console.log(error));
}






async function postWithCredentials(url) {
    return fetch(url,
        {
            method: "POST",
            //credentials: "include",
            headers: {
                "Authorization": getBearer()
            }
        });
}

async function postJsonWithCredentials(url, body) {
    return fetch(url,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": getBearer()
            },
            //credentials: 'include',
            body: JSON.stringify(body)
        })
        .catch(error => console.log(error));
}






async function patchJsonWithCredentials(url, body) {
    return fetch(url,
        {
            method: 'PATCH',
            headers: {
                "Content-Type": "application/json",
                "Authorization": getBearer()
            },
            //credentials: 'include',
            body: JSON.stringify(body)
        })
        .catch(error => console.log(error));
}

async function patchFormDataWithCredentials(url, formData) {
    return fetch(url,
        {
            method: 'PATCH',
            headers: {
                "Authorization": getBearer()
            },
            body: formData
        })
        .catch(error => console.log(error));
}






async function deleteWithCredentials(url) {
    return await fetch(url,
        {
            method: 'DELETE',
            headers: {
                "Authorization": getBearer()
            },
            //credentials: 'include'
        }).catch(error =>
            console.log(error)
        );
}






function getBearer() {
    return "Bearer " + getCookieByName('Bearer');
}