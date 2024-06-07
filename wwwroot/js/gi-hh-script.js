var dpList = [];
function validateDpNumber(dpNumber) {
    if (isExistDP(dpNumber))
        return;

    $.ajax({
        type: "get",
        url:"../GoodsIssue/ValidateDpNumber/" + dpNumber,
        success: function (data) {
            if (data === true) {
                $('button[type="submit"]').prop("disabled", false);
                //$('.field-validation-valid[data-valmsg-for="DPNumber"]').text("");
            }
        },
        error: function (error) {
            /*$('.field-validation-valid[data-valmsg-for="DPNumber"]').text("Invalid DP Number");*/
            $('button[type="submit"]').prop("disabled", true);
            $("input[name='DPNumber']").val('');
            removeDPList(dpNumber);

            var htmlText = $(`<div>`);
            htmlText.append(`<p class="text-danger">The scanned DP-${error.responseJSON[0].deliverY_NUMBER} location must be</p>`);

            error.responseJSON.forEach(data => {
                htmlText.append(`<p class="text-danger">Plant-${data.plant} Storage-${data.storage}</p>`);
            });
            
            Swal.fire({
                icon: 'error',
                title: `<span class="text-danger">DP Number doesn't match the location!</span>`,
                html: htmlText,
                confirmButtonColor: '#1877f2',
            });

        }

    });
}


function isExistDP(dpNumber) {

    if (dpNumber != "") {
        if (dpList.includes(dpNumber)) {
            return true;
        } else {
            dpList.push(dpNumber);
            return false;
        }
    }
    return true;    
}


function removeDPList(dpNumber) {
    let index = dpList.indexOf(dpNumber);
    if (index > -1) {
        dpList.splice(index, 1);
    }
}