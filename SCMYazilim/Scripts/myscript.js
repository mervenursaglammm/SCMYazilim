$(document).ready(function () {
    function getValueUsingClass() {
        var chkArray = [];
        / * Bir sınıfın 'chk' ekli tüm onay kutularını ara ve seçili olup olmadıklarını kontrol edin * /
        $(".chk:checked").each(function () {
            chkArray.push($(this).val());
        });

        /* virgulle td value degerleri listesi */
        var selected;
        selected = chkArray.join(',') + ",";

        if (selected.length > 1) {
            alert("Seçtiniz " + selected);
        } else {
            alert("Lütfen onay kutusundan en az bir tanesini seçiniz.");
        }
    }

    $("#companycheck").click(function () {
        if ($(this).is(":checked")) {
            $("#sirketAdi").show();
            $("#sirketKimligi").hide();
        } else {
            $("#sirketAdi").hide();
            $("#sirketKimligi").show();
        }
    });

    / * her onay kutusuna ekli olan sinif temel alinarak onay kutusu degerleri aliniyor * /
    $("#Hesapla").click(function () {
        getValueUsingClass();
        var topla = 0;

        $(".chk:checked").each(function () {
            topla += parseFloat($(this).attr("no"));
        });
        $("#tutar1").html(topla);
    });

});

     

