
$(document).ready(function () {
    $("#companycheck").click(function () {
        if ($(this).is(":checked")) {
            $("#sirketAdi").show();
            $("#sirketKimligi").hide();
        } else {
            $("#sirketAdi").hide();
            $("#sirketKimligi").show();
        }


    });
});


     

