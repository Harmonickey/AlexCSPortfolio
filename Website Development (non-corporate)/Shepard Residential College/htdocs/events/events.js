$(document).ready(function() {
    $('#calendar').fullCalendar({
        events: 'http://www.google.com/calendar/feeds/u.northwestern.edu_lo38cs76l0g82i1eu0qqksc2ps%40group.calendar.google.com/public/basic',
        aspectRatio: 1.5
    });
});