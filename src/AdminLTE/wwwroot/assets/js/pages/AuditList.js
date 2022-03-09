function GetAuditHistory(id) {
    $("#audit").html("");

    var AuditDisplay = "<table class='table table-condensed' cellpadding='5' style='table-layout:fixed; width:100%'>";
    $.getJSON("/SuperAdmin/audit/" + id, function (AuditTrail) {

        for (var i = 0; i < AuditTrail.length; i++) {
            AuditDisplay = AuditDisplay + "<tr class='active'><td colspan='2'>Event date: " + AuditTrail[i].auditDateTimeUtc + "</td>";
            AuditDisplay = AuditDisplay + "<td>Action Type: " + AuditTrail[i].auditType + "</td>";
            AuditDisplay = AuditDisplay + "<td>Table: " + AuditTrail[i].tableName + "</td>";
            /*            AuditDisplay = AuditDisplay + "<td>Table: " + AuditTrail[i].tableName + "</td>";*/
            AuditDisplay = AuditDisplay + "</tr>";
            AuditDisplay = AuditDisplay + "<tr class='active'><td colspan='2'>Username: " + AuditTrail[i].auditUser + "</td>";
            AuditDisplay = AuditDisplay + "<td>Action Row ID: " + AuditTrail[i].keyValues + "</td>";

            //if (AuditTrail[i].auditType = 'Update') {
            //    var changed = '';

            //    const changeObject = JSON.parse(AuditTrail[i].changedColumns);

            //    for (const key in changeObject) {
            //        // alert(changeObject[key])
            //        changed = changed + changeObject[key] + '<br />';
            //    }
            //    //alert(changed)
            //    AuditDisplay = AuditDisplay + "<td>Table: " + changed + "</td>";
            //   // AuditDisplay = AuditDisplay + "<td>Table: " + AuditTrail[i].changedColumns + "</td>";
            //}
            //else {
            //    AuditDisplay = AuditDisplay + "<td>Table: " + AuditTrail[i].changedColumns + "</td>";
            //}

            AuditDisplay = AuditDisplay + "<td>Table: " + AuditTrail[i].changedColumns + "</td>";
            AuditDisplay = AuditDisplay + "</tr>";
            AuditDisplay = AuditDisplay + "<td colspan='4'>"


            //  AuditDisplay = AuditDisplay + "</Div>";
            AuditDisplay = AuditDisplay + "<Div class='col-12'>";

            const oldObject = JSON.parse(AuditTrail[i].oldValues);
            const newObject = JSON.parse(AuditTrail[i].newValues);
            const values = [];


            for (const key in oldObject) {
                console.log(`${key}: ${oldObject[key]}`);
                values[key] = oldObject[key];
            }


            //AuditDisplay = AuditDisplay + "<table border='5px' bordercolor='#F35557' style='table-layout:fixed; width:100%'>";

            //AuditDisplay = AuditDisplay + "<h4 align='center'>OLD Table</h4>";
            //AuditDisplay = AuditDisplay + "<tr class='text-warning'>";
            //AuditDisplay = AuditDisplay + "<td>Field name</td>";
            //AuditDisplay = AuditDisplay + "<td>Old Value</td></tr>";
            //AuditDisplay = AuditDisplay + "</tr>";




            //for (const key in oldObject) {

            //    console.log(`${key}: ${oldObject[key]}`);
            //    AuditDisplay = AuditDisplay + "<tr>";            
            //    AuditDisplay = AuditDisplay + "<td>" + key + "</td>";
            //    AuditDisplay = AuditDisplay + "<td style='word-wrap: break-word'>" + oldObject[key] + "</td>";
            //    values[key] = oldObject[key];
            //    AuditDisplay = AuditDisplay + "</tr>";
            //}

            //if (typeof oldObject === 'undefined' || oldObject == null) {
            //    AuditDisplay = AuditDisplay + "<tr>";
            //    AuditDisplay = AuditDisplay + "<td>" + ' - ' + "</td>";
            //    AuditDisplay = AuditDisplay + "<td>" + ' - ' + "</td>";
            //    AuditDisplay = AuditDisplay + "</tr>";
            //}

            //AuditDisplay = AuditDisplay + "</table>" 
            //AuditDisplay = AuditDisplay + "</td>"
            //AuditDisplay = AuditDisplay + "<td colspan='2'>"


            //AuditDisplay = AuditDisplay + "</Div>";
            //AuditDisplay = AuditDisplay + "<Div class='col-6'>";


            AuditDisplay = AuditDisplay + "<table style='table-layout:fixed; width:100%'>";
            AuditDisplay = AuditDisplay + "<h4 align='center'>Change Details</h4>";
            AuditDisplay = AuditDisplay + "<tr class='text-warning'>";
            AuditDisplay = AuditDisplay + "<td>FIELD NAME</td>";
            AuditDisplay = AuditDisplay + "<td class='text-center'>OLD VALUE</td>";
            AuditDisplay = AuditDisplay + "<td class='text-center'>NEW VLAUE</td></tr>";
            AuditDisplay = AuditDisplay + "</tr>";



            for (const key in newObject) {

                console.log(`${key}: ${newObject[key]}`);
                AuditDisplay = AuditDisplay + "<tr>";
                AuditDisplay = AuditDisplay + "<td>" + key + "</td>";

                if (typeof oldObject === 'undefined' || oldObject == null) {
    
                    AuditDisplay = AuditDisplay + "<td>" + ' - ' + "</td>"; //old
                    AuditDisplay = AuditDisplay + "<td  class='text-center' >" + newObject[key] + "</td>"; //new
     
                }
                else {


                    AuditDisplay = AuditDisplay + "<td class='text-center'>" + oldObject[key] + "</td>";
                    //alert('OLD' + values[key]);
                    //alert('NEW' + newObject[key]);
                    var colour = '';
                    if (newObject[key] != values[key])
                    { AuditDisplay = AuditDisplay + "<td class='text-center alert-danger'>" + newObject[key] + "</td>"; }
                    else
                    { AuditDisplay = AuditDisplay + "<td  class='text-center' >" + newObject[key] + "</td>"; }

                }
                //AuditDisplay = AuditDisplay + "<td colour>" + newObject[key] + "</td>";

                //if (newObject[key] = values[key]) {
                //    AuditDisplay = AuditDisplay + "<td "class=alert-danger">" + newObject[key] + "</td>";
                //}
                //else {
                //    AuditDisplay = AuditDisplay + "<td>" + newObject[key] + "</td>";
                //}

                AuditDisplay = AuditDisplay + "</tr>";
            }


            //AuditDisplay = AuditDisplay + "</tr>";
        }
        AuditDisplay = AuditDisplay + "</table>"
        AuditDisplay = AuditDisplay + "</Div";
        AuditDisplay = AuditDisplay + "</Div>";


        AuditDisplay = AuditDisplay + "</td>"
        AuditDisplay = AuditDisplay + "</td>"
        AuditDisplay = AuditDisplay + "</tr>"
        AuditDisplay = AuditDisplay + "</table>"

        $("#audit").html(AuditDisplay);
        $("#myModal").modal('show');


    });
}

function textFromJson(json) {
    if (json === null || json === undefined) {
        return '';
    }
    if (!Array.isArray(json) && !Object.getPrototypeOf(json).isPrototypeOf(Object)) {
        return '' + json;
    }
    const obj = {};
    for (const key of Object.keys(json)) {
        obj[key] = textFromJson(json[key]);
    }
    return Object.values(obj).join(' ');
}
////function GetAuditHistory(recordID) {
////    $("#audit").html("");
////    var AuditDisplay = "<table class='table table-condensed' cellpadding='5'>"; // should be done with cleaner dom object construction ... but hey, its an article, not production :)
////    $.getJSON("/SuperAdmin/audit/" + recordID, function (AuditTrail) {

////        for (var i = 0; i < AuditTrail.length; i++) {
////            AuditDisplay = AuditDisplay + "<tr class='active'><td colspan='2'>Event date: " + AuditTrail[i].auditDatTimeUtc + "</td>";
////            AuditDisplay = AuditDisplay + "<td>Action type: " + AuditTrail[i].auditType + "</td></tr>";
////            AuditDisplay = AuditDisplay + "<tr class='text-warning'><td>Field name</td><td>New Values</td><td>After change</td></tr>";

////            var data = JSON.parse(AuditTrail[i].oldValues);
////            var data2 = JSON.parse(AuditTrail[i].newValues);

////            //for (var j = 0; j < data.count; j++) {
////            //    AuditDisplay = AuditDisplay + "<tr>";
////            //    AuditDisplay = AuditDisplay + "<td>" + data[j].id + "</td>";
////            //    AuditDisplay = AuditDisplay + "<td>" + data[j] + "</td>";
////            //    AuditDisplay = AuditDisplay + "<td>" + AuditTrail[i].newValues[j].ValueAfter + "</td>";
////            //    AuditDisplay = AuditDisplay + "</tr>";
////            //}

////            //for (var j = 0; j < data2.length; j++) {
////            //    AuditDisplay = AuditDisplay + "<tr>";
////            //    AuditDisplay = AuditDisplay + "<td>" +data2[j].id + "</td>";
////            //    AuditDisplay = AuditDisplay + "<td>" + AuditTrail[i].newValues[j].ValueBefore + "</td>";
////            //    AuditDisplay = AuditDisplay + "<td>" + AuditTrail[i].newValues[j].ValueAfter + "</td>";
////            //    AuditDisplay = AuditDisplay + "</tr>";
////            //}

////            //Object.entries(data).forEach(([key, value]) => {
////            //    console.log(`${key}: ${value}`)
////            //    AuditDisplay = AuditDisplay + "<td>" + '${key}' + "</td>";
////            //    AuditDisplay = AuditDisplay + "<td>" + '${value}' + "</td>";
////            //});

////            Object res = JsonConvert.DeserializeObject(AuditTrail[i].oldValues)

////            for (Object key in res) {
////                //if (obj.hasOwnProperty(key)) {
////                    AuditDisplay = AuditDisplay + "<tr>";
////                    AuditDisplay = AuditDisplay + "<td>" + `${key} : ${res[key]}` + "</td>";
////                    AuditDisplay = AuditDisplay + "</tr>";

////                    console.log(`${key} : ${res[key]}`)
////                //}
////            }

////        }
////        AuditDisplay = AuditDisplay + "</table>" >
////            $("#audit").html(AuditDisplay);
////        $("#myModal").modal('show');


////    });
////}

function DeleteRecord(recordID) {
    $.get("/home/delete/" + recordID); // should have return on success etc
    $('#' + recordID).remove();
}

function ShowDeleted() {
    window.location.assign('/?ShowDeleted=True');
}

function NewRecord() {
    window.location.assign('/home/create');
}


/*
var data = [{"id":"17b78bad-876d-49c4-89d0-53a8cb165e06","first_name":"Johny","middle_name":"233.86.192.59","last_name":"Scapens","gender":null,"email":"jscapens0@globo.com","website":"http://disqus.com/nisl.jpg","city":"Chandmanĭ","country":"MN","postal_code":null,"street_number":"967","street_name":"Grasskamp"},
{"id":"72a22bb0-d82e-4ffb-8392-3fdda288fb86","first_name":"Udale","middle_name":null,"last_name":"Paolillo","gender":"M","email":"upaolillo1@icio.us","website":"https://cnet.com/in/tempor/turpis.js","city":"Fort Worth","country":"US","postal_code":"76129","street_number":"389","street_name":"Forster"}]
function tableData (data) {
    
    //Creates the header using keys from JSON
    let objectKeys = Object.keys(data[0]);
    let table = document.querySelector('table');
    let headerRow = document.createElement ('tr');
    
    for (i=0; i < objectKeys.length; i++) {
        let headerCell = document.createElement('th');
        if (objectKeys[i].startsWith("first")) {
            headerCell.append(document.createTextNode("Not First Name"));
        } else {
            headerCell.append(document.createTextNode(objectKeys[i]));
        }
        
        headerRow.append(headerCell);
    };
    
    table.append(headerRow);
    
};
*/


//Get JSON, run function

fetch("./data/contacts.json")
    .then(function (response) {
        return response.json();
    }).then(function (json) {
        tableData(json);
    });

// Function

function tableData(data) {

    // Create Headings

    let objectKeys = Object.keys(data[0]);
    let table = document.querySelector('table');
    let headerRow = document.createElement('tr');

    for (let keyName of objectKeys) {
        let headerCell = document.createElement('th');
        headerCell.append(document.createTextNode(keyName));
        headerRow.append(headerCell);
    };

    table.append(headerRow);

    // Table Data

    for (let i = 0; i < data.length; i++) {
        let dataRow = document.createElement('tr');

        for (let keyName of objectKeys) {
            let dataCell = document.createElement('td');
            if (data[i][keyName] === null) {
                dataCell.append(document.createTextNode("Not Known"));
            } else if (data[i][keyName].startsWith("data:image")) {
                let img = document.createElement('img');
                img.src = data[i][keyName];
                dataCell.append(img);
            } else {
                dataCell.append(document.createTextNode(data[i][keyName]));
            };

            dataRow.append(dataCell);
        };

        table.append(dataRow);
    }
};