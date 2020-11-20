  
function myFunction() {
    var checkBox = document.getElementById("companycheck");
    // Get the output text
    var text = document.getElementById("sirketKimligi");
    var companyName = document.getElementById("companyName");

    // If the checkbox is checked, display the output text
    if (checkBox.checked == true) {
        text.style.display = "none";
        companyName.style.display = "block";
    } else {
        text.style.display = "block";
        companyName.style.display="none";
    }
}

