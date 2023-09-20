# NationalWeatherAlert
This is a C# web application using .Net Framework 6to display alerts from National Weather Service's API. It consists of 2 views: one to select the region and next to select state with forecast zone. The app will retrieve and display information about active alerts for it through ajax call to api to get response and bind it in table view.

This is a project that displays alerts from the National Weather Service's API.  It is focused on the 300 largest cities in the U.S.. The app will consist of two pages (or views).  The first lets the user select the region they wish to look into.  The next allows selection of a state followed by a forecast zone.   After selecting a zone, the app will retrieve and display information about active alerts for it.

Requirements

1) Setup
	a) Create a web application using any .net framework.

2) Page / View 1
	a) On the first page, display a dropdown that allows selection of a region
	b) The format of the text should be the region name, and then in parentheses next to it, the comma separated abbreviations of each state in the region.  Ex.: Southeast (AL,FL,GA,MS,TN) 
	c) There should also be a button that says 'Alerts'.  On clicking alerts, should load view 2.  
	Note, view 2 can display on the same page if you want, but this must be a call back to the server.

3) Page / View 2
	a) should have a dropdown with the states of the region selected in view 1.  
	b) All states should display whether or not they have city records.
	c) On selecting a state, should display the zones associated with that state (or empty if none).
	d) The zones should be grouped by the city they're in, with the city name as the group label
	e) On clicking a zone, call the NWS API and find all active alerts for the zone.  
		Note, this must not postback to the server
	f) Display the returned alerts in any kind of table.  Display the fields event, severity, headline, and description

Notes:

1) The endpoint to call is https://api.weather.gov/alerts/active/zone/<zone id>.  The zone id is the property Zone in the cities_and_zones data
2) The endpoint does not need authentication.
3) The endpoint responds with a single object.  The property features is the array of alerts.  On each feature, there is an object property called properties, and that has the individual fields we want to show.  Ex: responseData.features[0].properties.event
4) This was designed to not necessarily need any additional libraries.  That said, feel free to use any you choose.
5) Washington D.C. is a city in the data, and the District of Columbia is included as a 'state' in the data.
6) This doesn't need to be pretty, just functional.
