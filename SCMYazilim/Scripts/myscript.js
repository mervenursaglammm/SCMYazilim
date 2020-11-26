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
          // alert("Seçtiniz " + selected);
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
        
        var today = new Date();
        var lastDayOfMonth = new Date(today.getFullYear(), today.getMonth() + 1, 0);
        //alert(lastDayOfMonth.getDate());
        var d = new Date();
        //alert(d.getDate());
        var tarihFarki = lastDayOfMonth.getDate() - d.getDate() + 1;
        //alert(tarihFarki);
        $(".chk:checked").each(function () {
           topla += parseFloat($(this).attr("no"));
        });
        var tutar = topla * tarihFarki;
        $("#tutar1").html(tutar + "₺");
        var aciklama ="("+ d.getDate() + "." + (d.getMonth() + 1) + "." + d.getFullYear() + " tarihinden " + lastDayOfMonth.getDate() + "." + (lastDayOfMonth.getMonth() + 1) + "." + lastDayOfMonth.getFullYear() + " tarihine kadar tutarı " + tutar + "₺'dir.)";
        $("#aciklama").html(aciklama);
       // alert(d.getMonth());

       

       
    });

});

     

