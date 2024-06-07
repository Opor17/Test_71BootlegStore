// scan batch and pull data from pls
var batchList = [];
$('#GRProduction_Batch').on('keyup', function () {
    let batchNo = $(this).val();
    if (batchNo.length >= 10) {
        loadDataFromPls(batchNo);
    }
});
function loadDataFromPls(batchNo) {
    if (isExistDP(batchNo))
        return;

    $.ajax({
        type: "GET",
        url: routeBatchData,
        data: {
            batchNo
        },
        success: function (data) {
            $('#GRProduction_Batch').val('');
            removeBatchList(data.batch);
            confirmation(data);
        },
        error: function (error) {
            //if (error.status == "404") {
            //    $('.field-validation-valid[data-valmsg-for="GRProduction.Batch"]').text(error.responseText);
            //}

            let htmlText = `<p class="text-danger">${error.responseText}</p>`;
            Swal.fire({
                icon: 'error',
                title: `<span class="text-danger">Invalid data!</span>`,
                html: htmlText,
                confirmButtonColor: '#1877f2',
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.reload();
                }
            });
        }
    });
}


function confirmation(data) {

    if (data.isExist) {
        Swal.fire({
            title: "Are you sure?",
            text: `Batch Number ${data.batch} has been scanned.Do you want to override it ?`,
            icon: "info",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, override it!"
        }).then((result) => {
            if (result.isConfirmed) {
                for (const key in data) {
                    $('#GRProduction_' + capitalizeFirstLetter(key)).val(data[key]);
                }
            }
        });
    } else {
        for (const key in data) {
            $('#GRProduction_' + capitalizeFirstLetter(key)).val(data[key]);
        }
    }

    if (data.batchStatus !== 'A') {
        $('#GRProduction_BatchStatus').addClass('border-danger');
    } else {
        $('#GRProduction_BatchStatus').removeClass('border-danger');
    }

}

function isExistDP(batchNo) {

    if (batchNo != "") {
        if (batchList.includes(batchNo)) {
            return true;
        } else {
            batchList.push(batchNo);
            return false;
        }
    }
    return true;
}

function removeBatchList(batchNo) {
    let index = batchList.indexOf(batchNo);
    if (index > -1) {
        batchList.splice(index, 1);
    }
}


// delete datatable item
$(document).on('click', '.js-delete', function () {
    let itemId = $(this).attr('data-id');
    let batchNo = $(this).attr('data-batch');
    Swal.fire({
        title: 'Are you sure?',
        text: "You want to remove this batch " + batchNo,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            deleteItem(itemId)
        }
    });
});
function deleteItem(id) {
    $.ajax({
        type: "DELETE",
        url: routeDelete + "/" + id,
        success: function (data) {
            if (data == 'success') {
                window.location.reload();
            }
        },
        error: function (error) {
            console.log(error);
        },
    });
}


// send to SAP
$('#btnSendToSAP').on('click', function () {
    batchList = [];
    $("#receiveTable tbody tr").each(function () {
        batchList.push($(this).attr('data-batch_id'));
    });

    if (batchList.length === 0) {
        alert("Not found data to send");
        return;
    }

    $.ajax({
        type: "POST",
        url: routeSendtoSAP,
        data: {
            batchList
        },
        success: function (response) {
            var htmlText = $(`<div>`);

            response.forEach(data => {
                if (data.statusType == "S") {
                    htmlText.append(`<p class="text-dark mt-3">Batch- ${data.batchNo}, DocNo- ${data.materialDocNo}</p>`);
                } else {
                    htmlText.append(`<p class="text-danger mt-3">Batch- ${data.batchNo}, Message- ${data.errorMessage}</p>`);
                }
            });

            Swal.fire({
                icon: 'info',
                title: `<span class="text-success">SAP RESPONSE.</span>`,
                html: htmlText,
                confirmButtonColor: '#1877f2',
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.reload();
                }
            });
        },
        error: function (error) {
            let htmlText = `<p class="text-dark">${error.responseText}</p>`;
            Swal.fire({
                icon: 'error',
                title: `<span class="text-danger">Invalid data!</span>`,
                html: htmlText,
                confirmButtonColor: '#1877f2',
            });
        }
    });
});


function capitalizeFirstLetter(str) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}