


var captchaValue = '';

var onloadCallback = function (obj) {
    captchaValue = obj;
    checkBtnSubmit()
    
};

function pad(str, max) {
    str = str.toString();
    str = str.length < max ? pad("0" + str, max) : str; // zero à esquerda
    str = str.length > max ? str.substr(0, max) : str; // máximo de caracteres
    return str;
}

var checkBtnSubmit = function () {
    debugger;
    if ($('#AcceptsTermUse').prop('checked') == true &&
        $('#Consentimento').prop('checked') == true && captchaValue.length > 50) {
        $('#btnSubmit').removeAttr('disabled');
        $('#btnSubmit').removeClass('disabled');
    } else {
        $('#btnSubmit').attr('disabled', 'disabled');
        $('#btnSubmit').addClass('disabled');
    }
};


var expirationCallbackCaptcha = function (obj) {
    captchaValue = '';
    $('#btnSubmit').attr('disabled', 'disabled');
    $('#btnSubmit').addClass('disabled');
};


$(document).ready(function () {   

    $('#CompanyId-error').hide()
    $('#OccupationAreaClientUniqueIdentifier-error').hide()
    $('#CityId-error').hide()
    $('#StateId-error').hide()



    changeCompanyDescription();

    $("#divCompanyDescription").hide();

    loadCity($("#StateId").val());

    $('.materialSelect').on('contentChanged', function () {
        $(this).material_select();
    });

    $("#AcceptsTermUse").click(function () {
        $('#AcceptsTermUse').prop('checked', false);
        $('#modal1').modal('open');
        checkBtnSubmit();
    });

    $("#Consentimento").click(function () {
        checkBtnSubmit();
    });


    function changeCompanyDescription() {
        let companyId = $("#CompanyId").val();
        
        if (companyId != '9999999') {
            $("#divCompanyDescription").hide();
            $("#CompanyDescription").val(($('#CompanyId').find('option:selected').text()));
        }
        else {
            $("#divCompanyDescription").show();
            $("#CompanyDescription").val('');
        }
    }

    $("#CompanyId").change(function () {        
        changeCompanyDescription();       
    });

    $('.errorProfessionalHelth').hide();
    $('select').material_select();
    document.querySelectorAll('.select-wrapper').forEach(t => t.addEventListener('click', e => e.stopPropagation()))
    $('.modal').modal();
    $('.materialSelect').material_select();
    $(".progressCity").hide();    

    var SPMaskBehavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    },
        spOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(SPMaskBehavior.apply({}, arguments), options);
            }
        };

    $('#Phone').mask(SPMaskBehavior, spOptions);
  
    $('#CouncilNumber').keyup(function () {
        var newValue = '';
        if (($('#OccupationAreaClientUniqueIdentifier').find('option:selected').text() == 'Médico')){
            var valor_bruto = $('#CouncilNumber').val().replace(/[^\d]/g, '');
            var value = parseInt(0 + valor_bruto);
            newValue = pad(value, 7);
        }
        else {
            newValue = $('#CouncilNumber').val().replace(/[^\d]/g, '');
        }        
        $('#CouncilNumber').val(newValue);
    });

    
    
    $(".btnAccept").click(function () {
        $('#AcceptsTermUse').prop('checked', true);
        $('#modal1').modal('close');
        checkBtnSubmit()
    });

    $(".btnReject").click(function () {
        checkBtnSubmit()
    });

    $("#CityId").change(function () {        
        $("#CityDescription").val(($('#CityId').find('option:selected').text()));        
    })

    

    $("#OccupationAreaClientUniqueIdentifier").change(function () {
        $('#CouncilNumber').val('');
    });

    $("#StateId").change(function () {
        var idState = $(this).val();
        loadCity(idState);        
    });

    function loadCity(stateId) {
        $("#StateDescription").val(($('#StateId').find('option:selected').text()));
        $("#CityId").empty();
        $(".progressCity").show();
        $.getJSON("/ProfessionalRegistration/GetCity?stateId=" + stateId, function (data) {
            $("<option disabled selected>Selecione</option>").appendTo("#CityId");
            for (var i = 0; i < data.length; i++) {
                var $newOpt = $("<option>").attr("value", data[i].nome).text(data[i].nome)
                $("#CityId").append($newOpt);
            }
            $('#CityId').material_select();
            document.querySelectorAll('.select-wrapper').forEach(t => t.addEventListener('click', e => e.stopPropagation()))
            $(".progressCity").hide();
        });
    }

    $('form').submit(function () {

        var isValid = $("form").valid()

        debugger;
        var hasError = false 

        if (hasError)
            return false;

        if ($('#AcceptsTermUse').prop('checked') == false && $('#PasswordConfirmation').val() != '') {
            $('#modalAcceptUse').modal('show');
            return false;
        }       


        if (isValid) {
            $('#btnSubmit').attr('disabled', 'disabled');
            $('#btnSubmit').addClass('disabled');
        }

        return true;
       
        
    });

    //$("#IsHealthProfessional").click(function () {
    //    if ($('#IsHealthProfessional').prop('checked')) {
    //        $('.errorProfessionalHelth').hide();
    //    } else {
    //        $('.errorProfessionalHelth').show();

    //    }
    //});

});