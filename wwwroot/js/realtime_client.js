var conn = new signalR.HubConnectionBuilder().withUrl("/rtchat").build()

function makeTD(text, searchable = false, classes = "") {
    const el = document.createElement("td");
    el.innerHTML = text
    el.className = classes
    if(searchable) {
        el.setAttribute("data-searchable", "yes")
    }
    return el
}

conn.on("RTChatMsg", async (imsg) => {
    console.log("[RT]", imsg)
    const tr = document.createElement("tr");

    let isPrivate = false
    if (imsg.recipient !== undefined) {
        // private message
        isPrivate = true
        tr.className += "bg-warning bg-gradient"
    }
    
    const nameSpan = document.createElement("span")
    const formatTime = new Date(imsg.timestamp).toLocaleString().replace(",", "")
    const senderName = await fetch("/Helper/UsernameFromUUID?uuid=" + imsg.sender.id)
    const playerLink = document.createElement("a")
    playerLink.href = "/Admin/ManagePlayer?id=" + imsg.sender.id
    
    if(imsg.sender.customName.length > 0) {
        playerLink.innerText = `${imsg.sender.customName} (${await senderName.text()})`
    } else {
        playerLink.innerText = await senderName.text()
    }
    
    nameSpan.appendChild(playerLink)
    
    if(isPrivate) {
        let arrowSpan = document.createElement("span")
        arrowSpan.innerHTML = "&rarr;"
        nameSpan.appendChild(arrowSpan)
        
        let recipName = await fetch("/Helper/UsernameFromUUID?uuid=" + imsg.recipient.id)
        let recipLink = document.createElement("a")
        recipLink.href = "/Admin/ManagePlayer?id=" + imsg.recipient.id

        if(imsg.recipient.customName.length > 0) {
            recipLink.innerText = `${imsg.recipient.customName} (${await recipName.text()})`
        } else {
            recipName.innerText = await recipName.text()
        }
        
        nameSpan.appendChild(recipLink)
    }
    
    tr.appendChild(makeTD(formatTime))
    tr.appendChild(makeTD(nameSpan.outerHTML, true))
    tr.appendChild(makeTD(imsg.server, true))
    tr.appendChild(makeTD(imsg.content, true))
    
    if(isPrivate) {
        let el = makeTD("private", true)
        el.style.display = "none"
        tr.appendChild(el)
    }

    $("#evlog-table-body").prepend(tr)
})

conn.start().then(() => {
    console.log("Realtime chat active!")
}).catch(err => {
    console.error(err.toString());
});
