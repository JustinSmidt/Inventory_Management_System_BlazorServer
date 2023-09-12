function PreventFormSubmission(formId) {
    document.getElementById(`${formId}`).addEventListener("keydown", function (event) {
        if (event.keyCode == 13) {          //keycode 13 means Enter has been pushed
            event.preventDefault();
            return false;
        }
    })
}


//eventListener is going to listen for a keydown event, when keydown occurs, it triggers function (event), that then checks if
//Enter is pressed. 