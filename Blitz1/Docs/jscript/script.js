var captionItems = [];

function onscrollHandler() {
    activeElementSet();
}

function onresizeHandler() {
    activeElementSet();
}

function onclickHandler(event) {
    event = event || window.event;
    activeElementSet(event.currentTarget, captionItems);
}

function init() {
    window.onscroll = onscrollHandler;
    window.onresize = onresizeHandler;

    captionItems = [];
    var content = document.querySelector("#content");
    fillChapterLevel(content, captionItems);
        
    initMenu();
}

function fillChapterLevel(parentItem, parentCollection) {
    var chapterItem = parentItem.querySelector(".chapter");
    while (chapterItem !== null) {
        if (chapterItem.className === "chapter") {
            var captionItem = {
                caption: chapterItem.children[0].innerText,
                contentItem: chapterItem,
                menuItem: null,
                ref: "#" + chapterItem.attributes["id"].value,
                subitems: []
            };
            parentCollection.push(captionItem);
            fillChapterLevel(chapterItem, captionItem.subitems);
        }

        chapterItem = chapterItem.nextSibling;
    }
}

function initMenu() {
    var menuElement = document.getElementById("menu");
    while (menuElement.hasChildNodes()) {
        menuElement.removeChild(menuElement.firstChild);
    }

    var menuItem = document.createElement("UL");
    menuElement.appendChild(menuItem);
    menuElement = menuItem;

    createMenuItems(menuElement, captionItems, 1);

    captionItems[0].menuItem.setAttribute("id", "active");
    window.location = captionItems[0].ref;
}

function createMenuItems(parentElement, listItems, level) {
    for (var i = 0; i < listItems.length; i++) {
        var menuItem = document.createElement("LI");
        parentElement.appendChild(menuItem);
        menuItem.onclick = onclickHandler;

        var menuLink = document.createElement("A");
        menuItem.appendChild(menuLink);
        menuLink.setAttribute("href", listItems[i].ref);
        menuLink.innerText = listItems[i].caption;
        menuLink.className = "level" + level;
        listItems[i].menuItem = menuLink;

        createMenuItems(parentElement, listItems[i].subitems, level + 1);
    }
}

function topElementFind(itemList) {
    var returnValue = null;

    var position = document.body.scrollTop || document.documentElement.scrollTop || window.pageYOffset || 0;

    for (var i = 0; i < itemList.length; i++) {
        var item = itemList[i];

        if (position >= item.contentItem.offsetTop &&
            position < item.contentItem.offsetTop + item.contentItem.offsetHeight) {

            var item1 = topElementFind(item.subitems);
            if (item1 === null) {
                returnValue = item;
            }
            else {
                returnValue = item1;
            }
            break;
        }
    }

    return returnValue;
}

function activeElementSet(activeElement, elements) {
    if (activeElement === undefined) {
        var topElement = topElementFind(captionItems);
        if (topElement !== null) {
            activeElementSet(topElement.menuItem.parentElement, captionItems);
        }
    }
    else {
        for (var i = 0; i < elements.length; i++) {
            if (elements[i].menuItem.parentElement === activeElement) {
                elements[i].menuItem.setAttribute("id", "active");
            }
            else {
                elements[i].menuItem.setAttribute("id", "");
            }

            activeElementSet(activeElement, elements[i].subitems);
        }
    }
}