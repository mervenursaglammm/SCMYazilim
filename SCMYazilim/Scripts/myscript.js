  
function myFunction() {
    var checkBox = document.getElementById("companycheck");
    // Get the output text
    var text = document.getElementById("sirketKimligi");

    // If the checkbox is checked, display the output text
    if (checkBox.checked == true) {
        text.style.display = "block";
    } else {
        text.style.display = "none";
    }
}

