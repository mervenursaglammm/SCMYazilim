$(document).ready(function () {
    $('#file-input').change(function () {

        if (this.files && this.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#profile-img-tag').attr('src', e.target.result);

            }
            reader.readAsDataURL(this.files[0]);

            var name = event.target.files[0].name;
  

            if (window.FormData == undefined)
                alert("Error: FormData is undefined");

            else {
                var fileUpload = $("#file-input").get(0);
                var files = fileUpload.files;

                var fileData = new FormData();

                fileData.append(files[0].name, files[0]);

                $.ajax({
                    url: '/Home/uploadFile',
                    type: 'post',
                    datatype: 'json',
                    contentType: false,
                    processData: false,
                    async: false,
                    data: fileData,
                    success: function (response) {
                        // alert(response);

                    }

                });
            }

        }

    });
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
       // var aciklama ="("+ d.getDate() + "." + (d.getMonth() + 1) + "." + d.getFullYear() + " tarihinden " + lastDayOfMonth.getDate() + "." + (lastDayOfMonth.getMonth() + 1) + "." + lastDayOfMonth.getFullYear() + " tarihine kadar tutarı " + tutar + "₺'dir.)";
     
       // alert(d.getMonth());

        var bakiye = parseFloat($("#deneme").attr("no"));
        var aciklama = "";
        var lastDayOfNextMonth = new Date(today.getFullYear(), today.getMonth() + 2, 0);
        var sonrakiAyTutari = topla * lastDayOfNextMonth.getDate();
        if (bakiye >= tutar) {
          
            aciklama = "Belirttiğiniz seçeneklere göre " + d.getDate() + "." + (d.getMonth() + 1) + "." + d.getFullYear() + " tarihinden " + lastDayOfMonth.getDate() + "." + (lastDayOfMonth.getMonth() + 1) + "." + lastDayOfMonth.getFullYear() + " tarihine kadar kullanabilirsiniz." +
                " <br> Bir sonraki ay programı kullanmaya devam etmek için en az " + sonrakiAyTutari +"₺ ödeme yapmanız gerekir.";

        }
        else {
            aciklama = "Bakiyeniz yetersiz olduğu için işleminiz gerçekleştirilemedi.";
        }
        $("#aciklama").html(aciklama);
    


       
    });

});

     

