"use strict";

// function that creates dummy data for demonstration
function createDummyData() {
	var date = new Date();
	var data = {};

	for (var i = 0; i < 10; i++) {
		data[date.getFullYear() + i] = {};

		for (var j = 0; j < 12; j++) {
			data[date.getFullYear() + i][j + 1] = {};

			for (var k = 0; k < Math.ceil(Math.random() * 10); k++) {
				var l = Math.ceil(Math.random() * 28);

				try {
					data[date.getFullYear() + i][j + 1][l].push({
						startTime: "10:00",
						endTime: "12:00",
						text: "Some Event Here"
					});
				} catch (e) {
					data[date.getFullYear() + i][j + 1][l] = [];
					data[date.getFullYear() + i][j + 1][l].push({
						startTime: "10:00",
						endTime: "12:00",
						text: "Some Event Here"
					});
				}
			}
		}
	}

	return data;
}

// creating the dummy static data
var data = createDummyData();

// initializing a new calendar object, that will use an html container to create itself
var calendar = new Calendar("calendarContainer", // id of html container for calendar
	"medium", // size of calendar, can be small | medium | large
	[
		"Monday", // left most day of calendar labels
		3 // maximum length of the calendar labels
	], [
	"#486CCE", // primary color
	"#3854A0", // primary dark color
	"#ffffff", // text color
	"White" // text dark color
],
	{
		// placeholder: "" // Removes Organizer's Placeholder
		placeholder: "<button style='width: calc(100% - 16px); background-color: #E6E6E6; border-radius: 6px; margin: 8px; border: none; padding: 12px 0px; cursor: pointer;'>Add New Event</button>",
		indicator: true,
		indicator_type: 0, // indicator type, can be 0 (not numeric) | 1 (numeric)
		indicator_pos: "top" // indicator position, can be top | bottom
	}
);

// initializing a new organizer object, that will use an html container to create itself
var organizer = new Organizer("organizerContainer", // id of html container for calendar
	calendar, // defining the calendar that the organizer is related to
	data // giving the organizer the static data that should be displayed
);

//// This is gonna be similar to an ajax function that would grab
//// data from the server; then when the data for a this current month
//// is grabbed, you just add it to the data object of the form
//// { year num: { month num: { day num: [ array of events ] } } }
//function dataWithAjax(date, callback) {
//	var data = {};

//	try {
//		data[date.getFullYear()] = {};
//		data[date.getFullYear()][date.getMonth() + 1] = serverData[date.getFullYear()][date.getMonth() + 1];
//	} catch (e) { }

//	callback(data);
//};

//window.onload = function () {
//	dataWithAjax(new Date(), function (data) {
//		// initializing a new organizer object, that will use an html container to create itself
//		organizer = new Organizer("organizerContainer", // id of html container for calendar
//			calendar, // defining the calendar that the organizer is related
//			data // small part of the data of type object
//		);

//		// after initializing the organizer, we need to initialize the onMonthChange
//		// there needs to be a callback parameter, this is what updates the organizer
//		organizer.onMonthChange = function (callback) {
//			dataWithAjax(organizer.calendar.date, function (data) {
//				organizer.data = data;
//				callback();
//			});
//		};
//	});
//};