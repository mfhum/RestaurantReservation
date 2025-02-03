import { useState } from "react";
import { AvailabilityByMonth } from "../../../api/models/availability.ts";
import classes from "./CustomDatePicker.module.sass";

function CustomDatePicker({
                            availability,
                            reservationTime,
                            onMonthChange,
                            onDayChange,
                          }: {
  availability: AvailabilityByMonth[];
  reservationTime: string;
  onMonthChange: (date: string) => void;
  onDayChange: (date: string) => void;
}) {
  const months = [
    "Januar", "Februar", "März", "April", "Mai", "Juni",
    "Juli", "August", "September", "Oktober", "November", "Dezember",
  ];
  const reservationDate = new Date(reservationTime);
  const daysOfWeek = ["Mo", "Di", "Mi", "Do", "Fr", "Sa", "So"];
  const [currentMonth, setCurrentMonth] = useState(reservationDate.getMonth());
  const [currentYear, setCurrentYear] = useState(reservationDate.getFullYear());
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);
  function getClassName(state?: number): string {
    switch (state) {
      case 0:
        return classes.inPast;
      case 1:
        return classes.today;
      case 2:
        return classes.open;
      case 3:
        return classes.booked;
      case 4:
        return classes.closed;
      default:
        return "";
    }
  }

  // Generate days for the calendar
  const getDaysInMonth = (month: number, year: number) => {
    const startDay = new Date(year, month, 1).getDay(); // Day of week (0=Sun)
    const daysInMonth = new Date(year, month + 1, 0).getDate(); // Total days
    const daysArray = [];

    // Empty slots before the first day
    for (let i = 0; i < (startDay === 0 ? 6 : startDay - 1); i++) {
      daysArray.push(null);
    }

    // Actual days in the month
    for (let i = 1; i <= daysInMonth; i++) {
      daysArray.push(new Date(year, month, i));
    }

    return daysArray;
  };

  const daysArray = getDaysInMonth(currentMonth, currentYear);

  // Notify parent about the new month
  function handleMonthChange(newMonth: number, newYear: number) {
    setCurrentMonth(newMonth);
    setCurrentYear(newYear);

    // Always notify the parent about the **first day of the new month**
    const newDate = new Date(newYear, newMonth, 1);
    newDate.setDate(newDate.getDate() + 1); // Add one day to avoid timezone issues
    onMonthChange(newDate.toISOString());
  }

  const handleNextMonth = () => {
    if (currentMonth === 11) {
      handleMonthChange(0, currentYear + 1); // Move to January of the next year
    } else {
      handleMonthChange(currentMonth + 1, currentYear); // Move to the next month
    }
  };

  const handlePrevMonth = () => {
    if (currentMonth === 0) {
      handleMonthChange(11, currentYear - 1); // Move to December of the previous year
    } else {
      handleMonthChange(currentMonth - 1, currentYear); // Move to the previous month
    }
  };

  function handleDayClick(date: Date) {
    setSelectedDate(date);
    onDayChange(date.toISOString());
  }

  return (
      <div className={classes.calendarContainer}>
        <h2 className={classes.calendarTitle}>Wähle einen Tag aus</h2>
        <div className={classes.calendarHeader}>
          <button onClick={handlePrevMonth} className={classes.navButton} disabled={new Date().getMonth() == currentMonth}>
            <p className={classes.monthSelector}>&larr; Vorheriger Monat</p>
          </button>
          <span className={classes.currentMonth}>
            <p>{months[currentMonth]} {currentYear}</p>
          </span>
          <button onClick={handleNextMonth} className={classes.navButton} disabled={new Date().getMonth() + 3 == currentMonth}>
            <p className={classes.monthSelector}>Nächster Monat  &rarr;</p>
          </button>
        </div>
        <div className={classes.calendarGrid}>
          {daysOfWeek.map((day) => (
              <span key={day} className={classes.dayHeader}>
            {day}
          </span>
          ))}
          {daysArray.map((date, index) =>
              date ? (
                  <button
                      key={index}
                      onClick={() => handleDayClick(date)}
                      className={`${classes.day} ${
                          selectedDate?.getTime() === date.getTime()
                              ? classes.selectedDay
                              : ""
                      }`}
                      disabled={
                          availability[date.getDate() - 1]?.state == 0 ||
                          [classes.closed, classes.booked, classes.today].includes(getClassName(availability[date.getDate() - 1]?.state))
                      }
                  >
                    <p>{date.getDate()}</p>
                    <div
                        className={getClassName(
                            availability[date.getDate() - 1]?.state
                        )}
                    />
                  </button>
              ) : (
                  <div key={index} className={classes.emptyDay}></div>
              )
          )}
        </div>
        {selectedDate && (
            <p className={classes.selectedDate}>
              Ausgewähltes Datum: {selectedDate.toLocaleDateString("de-DE")}
            </p>
        )}
      </div>
  );
}

export default CustomDatePicker;
