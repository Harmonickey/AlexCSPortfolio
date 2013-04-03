#-------------------------------------------------------------------------------
# Name:        module1
# Purpose:
#
# Author:      Alex
#
# Created:     04/12/2012
# Copyright:   (c) Alex 2012
# Licence:     <your licence>
#-------------------------------------------------------------------------------

def main():
    pass

if __name__ == '__main__':
    main()

def IsLeapYear(year):
    if (year % 400 == 0):
        return True
    elif (year % 100 == 0):
        return False
    elif (year % 4 == 0):
        return True
    else:
        return False

class date(object):
    """Calendar dates consisting of a day, a month, and a year"""
    def __init__(self, day, month, year):
        if (day < 1):
            day = abs(day) % 28
        if (month < 1):
            month = abs(month) % 12
        if (year < 1):
            year = abs(year)
        self.day = day
        self.month = month
        self.year = year


    def print(self):
        print ("/".join([str(self.month), str(self.day), str(self.year)]))

    def equal(self, date_2):
        """Returns true iif day, month, year are equal"""
        if (self.day == date_2.day and
            self.month == date_2.month and
            self.year == date_2.year):
                return True
        else:
            return False

    def before(self, date_2):
        """Returns true iif self is earlier than date_2"""
        if (self.year < date_2.year):
            return True
        elif(self.year == date_2.year):
            if (self.month < date_2.month):
                return True
            elif (self.month == date_2.month):
                if (self.day < date_2.day):
                    return True
        return False

    def after(self, date_2):
        """Returns true iif self is later than date_2"""
        if self.year > date_2.year:
            return True
        elif self.year==date_2.year:
            if self.month > date_2.month:
                return True
            elif self.month==date_2.month:
                if self.day > date_2.day:
                    return True
        return False


    def tomorrow(self):
        """Advances self by one day"""
        self.day += 1
        #Take care of days first
        if (self.month == 1 or
            self.month == 3 or
            self.month == 5 or
            self.month == 7 or
            self.month == 8 or
            self.month == 10 or
            self.month == 12):
                if (self.day == 32):
                    self.day = 1
        elif (self.month == 2):
            if (IsLeapYear(self.year)):
                if (self.day == 30):
                    self.day = 1
            else:
                if (self.day == 29):
                    self.day = 1
        else:
            if (self.day == 31):
                self.day = 1

        #now check if we've advanced enough days to increment month & year
        if (self.day == 1):
            self.month += 1
            if (self.month == 13):
                self.month = 1
                self.year += 1

    def yesterday(self):
        """Moves self back by one day"""
        #Anticipate if we're going to have to decrement month & year
        if (self.day == 1):
            self.month -= 1
            if (self.month == 0):
                self.month = 12
                self.year -= 1

        self.day -= 1
        #Now deal with the day number if the day we decremented turned out to be 0
        if (self.month == 2):
            if (IsLeapYear(self.year)):
                if (self.day == 0):
                    self.day = 29
            else:
                if (self.day == 0):
                    self.day = 28
        elif (self.month == 1 or
            self.month == 3 or
            self.month == 5 or
            self.month == 7 or
            self.month == 8 or
            self.month == 10 or
            self.month == 12):
                if (self.day == 0):
                    self.day = 31
        else:
            if (self.day == 0):
                self.day = 30

    def forward(self, n):
        """Moves self forward n days"""
        for i in range(0,n):
            self.tomorrow()

    def backward(self, n):
        """Moves self backward n days"""
        for i in range(0,n):
            self.yesterday()


    def diff(self, date_2):
        """Returns number of days difference between self and date_2"""
        difference = 0
        while (not self.equal(date_2)):
            if (self.before(date_2)):
                self.tomorrow()
                difference += 1
            elif (self.after(date_2)):
                self.yesterday()
                difference -= 1
        return difference

    def dow(self):
        """Returns the day of the week of self"""
        ref_date = date(5,12,2012)
        week_names = ["W", "Th", "F", "S", "Su", "M", "T"]
        week_names_inv = ["W", "T", "M", "Su", "S", "F", "Th"]
        difference = self.diff(ref_date)
        index = abs(difference) % 7
        if (difference < 0):
            return week_names[index]
        else:
            return week_names_inv[index]

new_date = date(1,1,1700)
print(new_date.dow())








