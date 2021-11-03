var array = [];

function addTagebuchEinträge(id, name, punkte, bewertung) {
    var img = "";
    array.push({"Id":id, "Name":name, "Punkte":punkte, "Bewertung":bewertung, "Img":img},);

    return array;
}

function updateTagebuchEinträge(bewertungID, img) {
    for (var item in array) {
        if (array[item].Bewertung == null) {
            array[item].Bewertung = bewertungID;
        }
        if (array[item].Img == null || array[item].Img == "") {
            array[item].Img = img;
        }
    }
}

function removeTagebuchEinträge(id) {
    for (var item in array) {
        if (id == array[item].Id) {
          array.splice(item, 1);
        }
    }
}

function getTagebuchEinträge() {
    return array;
}

function bewertung(data) { // onclick(this)
    const kategorie = localStorage.getItem("kategorie");
    var bewertungID = data.getAttribute("data-id");
    var nameImg = data.getAttribute("value");

    addSmileyEintrag(nameImg);
    updateTagebuchEinträge(bewertungID, nameImg);
    // console.log(array);
    hideBewertung();
}

  
function addSmileyEintrag(nameImg) {
    var elements = document.getElementsByClassName('item'); // get all elements

    const htmlSmiley = `
    <div class="smileyBewertung">
        <img src="./bilder/${nameImg}">
    </div>
    `;

    for (var item in array) {
        if (array[item].Bewertung == null) {
            for(var i = 0; i < elements.length; i++){
                if (array[item].Name == elements[i].getElementsByClassName("items")[0].innerHTML) {
                    // Smiley bewertung hinzufügen
                    elements[i].innerHTML += htmlSmiley;
                }
            }
        }
    }
  }

  function removeSmileyEintrag(name) {
    var elements = document.getElementsByClassName('item'); // get all elements

    for(var i = 0; i < elements.length; i++){
        if (name == elements[i].getElementsByClassName("items")[0].innerHTML) {
            // Smiley Bewertung entfernen
            elements[i].removeChild(elements[i].childNodes[5]);
        }
    }
  }