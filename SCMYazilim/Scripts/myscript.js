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

    $(".sec").click(function () {
        var SatirNo = $(".sec").attr("id");
        alert(SatirNo);
    });

    / * her onay kutusuna ekli olan sinif temel alinarak onay kutusu degerleri aliniyor * /
	$("#Hesapla").click(function () {
		alert("deneme");
        getValueUsingClass();
    });

    /* div 
    $("#buttonParent").click(function () {
        getValueUsingParentTag();
    }); */
});

function getValueUsingClass() {
	var chkArray = [];

	/ * Bir sınıfın 'chk' ekli tüm onay kutularını ara ve seçili olup olmadıklarını kontrol edin * /
	$(".chk:checked").each(function () {
		chkArray.push($(this).val());
	});

	/* "Alert" özelliğini kullanacağımız için virgülle güzelleştirelim :) */
	var selected;
	selected = chkArray.join(',') + ",";

	if (selected.length > 1) {
		alert("Seçtiniz " + selected);
	} else {
		alert("Lütfen onay kutusundan en az bir tanesini seçiniz..");
	}
}

//function getValueUsingParentTag() {
//	var chkArray = [];

//	/* Ona bağlı 'checkboxlist' adı verilen ve kontrol edilip edilmediğini kontrol eden tüm onay kutularını arayın */
//	$("#checkboxlist input:checked").each(function () {
//		chkArray.push($(this).val());
//	});

//	/ * Virgülle ayrılmış diziye ayırmak * /
//	var selected;
//	selected = chkArray.join(',') + ",";

//	if (selected.length > 1) {
//		alert("Seçtiniz " + selected);
//	} else {
//		alert("Lütfen onay kutusundan en az bir tanesini seçiniz..");
//	}
//} 


     

