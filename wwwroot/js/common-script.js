$('#dataTable').DataTable();

// new
$('#btnNew').on('click', function () {
	controlForm('new');
});


// edit
$(document).on('click', '.js-edit', function () {
	let id = $(this).attr('data-id');
	controlForm('edit', id);
});

// view
$(document).on('click', '.js-view', function () {
	let id = $(this).attr('data-id');
	controlForm('view', id);
});

// fn control form
function controlForm(action, id = undefined) {
	$('.field-validation-valid').text('');
	switch (action) {
		case 'edit':
			$('#frmTitle').text("Edit");
			$('#frmData input').prop('readonly', false);
			$('#btnSubmit').prop('disabled', false);
			$('#frmData ' + formName + 'Id').prop('disabled', false);
			getDataById(id);
			break;
		case 'view':
			$('#frmTitle').text("View");
			$('#frmData input').prop('readonly', true);
			$('#btnSubmit').prop('disabled', true);
			$('#frmData '+ formName+'Id').prop('disabled', false);
			getDataById(id);
			break;
		default:
			$('#frmTitle').text("New");
			$('#frmData input').prop('readonly', false);
			$('#btnSubmit').prop('disabled', false);
			$('#frmData input').val('');
			$('#frmData ' + formName + 'Id').prop('disabled', true);
			$('#frmData select').prop('selectedIndex', 0);
			break;
	}

}

// get data by id
function getDataById(id) {
	$.get(routeDetail+'/' + id, function (data) {
		for (var key in data) {
			if (key == "dbName") {
				$('#frmData ' + formName + "DBName").val(data[key]);
			} else {
				$('#frmData '+formName + capitalizeFirstLetter(key)).val(data[key]);
			}
		}
	});
}

// submit form
$('#btnSubmit').on('click', function () {
	let formData = new FormData($('#frmData')[0]);
	$.ajax({
		type: 'POST',
		url: routeSave,
		data: formData,
		dataType: 'json',
		processData: false,
		contentType: false,
		success: function (data) {
			window.location.reload();
		},
		error: function (error) {
			$('.field-validation-valid').text('');
			for (const key in error.responseJSON) {
				let formName = getFieldName(key);
				if (formName == "glAccount") {
					$('.field-validation-valid[data-valmsg-for="GLAccount.Name"]').text(error.responseJSON[key][0]);
				} else {
					$('.field-validation-valid[data-valmsg-for="' + capitalizeFirstLetter(key) + '"]').text(error.responseJSON[key][0]);
				}

			}
		}
	});
});

// delete data
$(document).on('click', '.js-delete', function () {
	const id = $(this).attr('data-id');
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
			deleteItem(id);
		}
	})

});
function deleteItem(id) {
	$.ajax({
		type: "DELETE",
		url: routeDelete + '/' + id,		
		success: function (data) {
			window.location.reload();
		},
		error: function (error) {
			console.log(error);
		}
	});
}

function capitalizeFirstLetter(str) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}

function getFieldName(value) {
	let result = value.split('.');
	return result[0];
}