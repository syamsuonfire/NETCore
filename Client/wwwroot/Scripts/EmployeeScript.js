var dateNow = new Date();
var Departments = [];
$(document).ready(function () {
    $('#Employee').DataTable({ //Nama table pada index
        "ajax": {
            url: "/Employee/LoadEmployee", //Nama controller/fungsi pada index controller
            type: "GET",
            dataType: "json",
            dataSrc: ""
        },
        "columnDefs": [
            { "orderable": false, "targets": 8 },
            { "searchable": false, "targets": 8 }
        ],
        "columns": [
            {
                "data": null, render: function (data, type, row) {
                    return data.firstName + ' ' + data.lastName;
                }
            },
            { "data": "departmentName" },
            { "data": "email", "name": "Email" },
            {
                "data": "birthDate", "render": function (data) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            { "data": "phoneNumber", "name": "Phone Number" },
            { "data": "address", "name": "Address" },
            {
                "data": "createDate", "render": function (data) {
                    return moment(data).format('DD MMMM YYYY, h:mm a');
                }
            },
            {
                "data": "updateDate", "render": function (data) {
                    var dateupdate = "Not Updated Yet";
                    var nulldate = null;
                    if (data === nulldate) {
                        return dateupdate;
                    } else {
                        return moment(data).format('DD MMMM YYYY, h:mm a');
                    }
                }
            },
            {
                data: null, render: function (data, type, row) {
                    debugger;
                    return " <td><button type='button' class='btn btn-warning' Id='Update' onclick=$('#EditBtn').show();GetById('" + row.email + "');>Edit</button> <button type='button' class='btn btn-danger' Id='Delete' onclick=Delete('" + row.email + "');>Delete</button ></td >";
                }
            },
        ]
    });

    // tooltip
    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })
    LoadDepartment($('#DepartmentOption'));
});


function Delete(Email) {
    debugger;
    Swal.fire({
        title: "Are you sure ?",
        text: "You won't be able to Revert this!",
        showCancelButton: true,
        confirmButtonText: "Yes, Delete it!"
    }).then((result) => {
        if (result.value) {
            debugger;
            $.ajax({
                url: "/Employee/Delete/",
                data: { Email: Email }
            }).then((result) => {
                debugger;
                if (result.statusCode == 200) {
                    Swal.fire({
                        icon: 'success',
                        position: 'center',
                        title: 'Delete Successfully',
                        timer: 5000
                    }).then(function () {
                       $('#Employee').DataTable().ajax.reload();
                        ClearScreen();
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Failed to Delete',
                    })
                }
            })
        }
    });
}



function Save() {
    var Employee = new Object(); // new object employee
    debugger;
    Employee.FirstName = $('FirstName').val();
    Employee.LastName = $('#LastName').val();
    Employee.Email = $('#Email').val();
    Employee.Password = $('#Password').val();
    Employee.BirthDate = $('#BirthDate').val();
    Employee.PhoneNumber = $('#PhoneNumber').val();
    Employee.Address = $('#Address').val();
    Employee.Department_Id = $('#DepartmentOption').val();

    $.ajax({
        type: 'POST',
        url: '/Employee/Insert/',
        data: Employee
    }).then((result) => {
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Employee Added Succesfully'
            }).then((result) => {
                if (result.value) {
                    $('#Employee').DataTable().ajax.reload();
                }
            });
        }
        else {
            Swal.fire('Error', 'Failed to Add Employee', 'error');
            ClearScreen();
        }
    })


}
debugger;
function GetById(Email) {
    $.ajax({
        url: "/Employee/GetByEmail/" + Email,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: { Email: Email },
        async: false,
        success: function (result) {
            debugger;
            const obj = JSON.parse(result);
            const obj2 = JSON.parse(obj);
            $('#email').val(obj2.data.email);
            $('#firstname').val(obj2.data.firstName);
            $('#lastname').val(obj2.data.lastName);
            $('#birthdate').val(moment(obj2.data.birthDate).format('YYYY-MM-DD'));
            $('#address').val(obj2.data.address);
            $('#phone').val(obj2.data.phoneNumber);
            $('#departmentoption').val(obj2.data.department_Id);
            $('#myModal').modal('show');
            $('#UpdateBtn').show();
            $('#SaveBtn').hide();

        },

        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

}

function Edit() {
    debugger;
    var Employee = new Object();
    Employee.Email = $('#Email').val();
    Employee.FirstName = $('#FirstName').val();
    Employee.LastName = $('#LastName').val();
    Employee.BirthDate = $('#BirthDate').val();
    Employee.PhoneNumber = $('#PhoneNumber').val();
    Employee.Address = $('#Address').val();
    Employee.Department_Id = $('#DepartmentOption').val();
    $.ajax({
        type: 'POST',
        url: 'Employee/Update',
        data: Employee
    }).then((result) => {
        if (result.statusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Employee Updated Succesfully'
            }).then((result) => {
                if (result.value) {
                    $('#Employee').DataTable().ajax.reload();
                }
            });
        }
        else {
            Swal.fire('Error', 'Failed to Update Employee', 'error');
            $('#myModal').modal('show');
            $('#SaveBtn').hide();
        }
    })
}




function ClearScreen() {
    $('#Id').val('');
    $('#Name').val('');
    $('#EditBtn').hide();
    $('#SaveBtn').show();
}


// Selectlist
function LoadDepartment(element) {
    if (Departments.length === 0) {
        $.ajax({
            type: "Get",
            url: "/Department/LoadDepartment",
            success: function (data) {
                Departments = data.data;
                renderDepartment(element);
            }
        });
    }
    else {
        renderDepartment(element);
    }
}

function renderDepartment(element) {
    var $option = $(element);
    $option.empty();
    $option.append($('<option/>').val('0').text('Select Department').hide());
    $.each(Departments, function (i, val) {
        $option.append($('<option/>').val(val.id).text(val.name));
    });
}