var plantName, dpNumber, materialNumber, serverId;

const intVal = function (i) {
    return typeof i === 'string'
        ? i.replace(/[\$,]/g, '') * 1
        : typeof i === 'number'
            ? i
            : 0;
};

$(document).ready(function () {

    $('#giTable').DataTable({
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 0
        }],
        select: {
            style: 'os',
            selector: 'td:first-child'
        },
        order: [
            [1, 'asc']
        ],
    });

    $("#displayTable").DataTable();
    $("#materialTable").DataTable();
    $("#viewWeightTable").DataTable();

});


// get plants list by server id
$(document).on('change', '#ServerId', function () {
    serverId = $(this).val();
    $.get('../../WEB/Server/GetPlantList?serverId=' + serverId, function (data) {
        $('#PlantName').html('');

        data.forEach(plant => {
            let option = `<option value="${plant.name}">${plant.name}</option>`;
            $('#PlantName').append(option);

            if (plant.isDefault) {
                $('select[name="PlantName"] option[value="' + plant.name +'"]').attr('selected', 'selected');
            }
        });
    });
});


// view material details
$("#giTable").on("click", "a", function () {
    dpNumber = $(this).attr('data-id');
    plantName = $('#PlantName').val();
    serverId = $('#ServerId').val();

    $('#displayModal #dpNumber').text(dpNumber);
    ViewDPDetail();
});


// view batch details
$("#displayTable").on("click", "a", function () {
    materialNumber = $(this).attr('data-id');
    plantName = $('#PlantName').val();
    serverId = $('#ServerId').val();
    dpNumber = $(this).parents('tr').attr('data-id');

    $('#materialModal #materialNo').text(materialNumber);
    ViewMaterialDetails();
});


$('#viewWeightTable tbody tr td a').on('click', function () {
    materialNumber = $(this).attr('data-item_id');
    dpNumber = $(this).parents('tr').attr('data-id'); 

    $('#materialModal #materialNo').text(materialNumber);
    ViewMaterialDetails();
});


// material list by dpNumber
function ViewDPDetail() {
    $("#displayTable").DataTable({
        destroy: true,
        colReorder: {
            realtime: true
        },
        ajax: {
            url: "../GoodsIssue/DPDetails",
            data: {
                serverId,
                dpNumber,
                plantName
            },
            dataSrc: ""            
        },
        "createdRow": function (row, data, dataIndex) {
            $(row).attr('data-id', data['deliverY_NUMBER']);
        },
        columns: [
            {
                data: "deliverY_ITEM_NO"
            },
            {
                data: "materiaL_NUMBER",
                render: function (data) {
                    return `<a class="text-primary font-bold" 
                        href="javascript:;"
                        data-id="${data}"
                        data-tw-toggle="modal" 
                        data-tw-target="#materialModal">
                        ${data}
                    </a>`;
                }
            },
            {
                data: "plant"
            },
            {
                data: "storage"
            },
            {
                data: "dN_ITEM_QTY"
            },
            {
                data: "saleS_UNIT"
            },
            {
                data: "neT_WEIGHT",
                className: 'text-end'
            },
            {
                data: "grosS_WEIGHT",
                className: 'text-end'
            },
            {
                data: "weighT_UNIT"
            }
        ]
    });

}


// batch list by material number
function ViewMaterialDetails() {
    $("#materialTable").DataTable({
        destroy: true,
        colReorder: {
            realtime: true
        },
        ajax: {
            url: "../GoodsIssue/MaterialDetails",
            data: {
                serverId,
                dpNumber,
                plantName,
                materialNumber
            },
            dataSrc: ""
        },
        columns: [
            {
                data: "batchNumber",
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                data: "batchNumber"
            },
            {
                data: "saP_WEIGHT",
                className: 'text-end'
            },
            {
                data: "hH_WEIGHT",
                className: 'text-end'
            }
        ],
        drawCallback: function () {
            var api = this.api();

            var sapAll = api.column(2, { page: 'current' }).data().reduce((a, b) => intVal(a) + intVal(b), 0).toFixed(3);
            var hhAll = api.column(3, { page: 'current' }).data().reduce((a, b) => intVal(a) + intVal(b), 0).toFixed(3);

            $(api.column(2).footer()).html(sapAll);
            $(api.column(3).footer()).html(hhAll);
        }

    });

}


// delete
$(document).on('click', '.js-delete', function () {
    Swal.fire({
        title: 'Are you sure?',
        text: "You want to remove this record!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            DeleteRow();
        }
    })
});


function DeleteRow() {
    let delGR = [];
    $("#giTable tbody tr").each(function (index) {
        let checkBox = $(this).find("input[type='checkbox']").prop("checked");

        if (checkBox === false) {
            return true;
        }

        delGR.push($(this).find("a").text());
        $(this).remove();
    });

    $.ajax(
        {
            url: routeDelete,
            type: 'POST',
            data: { delGR },
            dataType: 'json',
            success: function (result) {
                // Handle the success response
                window.location.reload();

            },
            error: function (error) {
                // Handle errors
                console.log("An error occurred:", error);
            }
        }
    );
}


// toggle checkbox for notification
$('#all').on('change', function () {
    if ($(this).prop('checked')) {
        $(this).closest('table').find('input:checkbox').prop('checked', true);
    } else {
        $(this).closest('table').find('input:checkbox').prop('checked', false);
    }
});