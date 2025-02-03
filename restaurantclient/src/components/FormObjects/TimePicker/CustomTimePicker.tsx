import { useState, useEffect } from "react";
import classes from "./CustomTimePicker.module.sass";
import { AvailabilityByDay } from "../../../api/models/availability.ts";

function CustomTimePicker({ availableTimes, onSelectTime }: {
  availableTimes: AvailabilityByDay[]; // Array of time strings (e.g., ["13:00", "13:15", ...])
  onSelectTime: (time: string) => void; // Callback when a time is selected
}) {
  const [selectedTime, setSelectedTime] = useState<string | null>(null);
  const [isOpen, setIsOpen] = useState(false);

  useEffect(() => {
  }, [selectedTime]);

  useEffect(() => {
    setSelectedTime(null);
  }, [availableTimes]);

  const handleSelectTime = (time: Date) => {
    const timeString = time.toISOString();
    setSelectedTime(timeString);
    onSelectTime(timeString);
    setIsOpen(false); // Close the dropdown after selecting
  };
  
  const addOneHour = (time: string) => {
    const date = new Date(time);
    date.setHours(date.getHours() + 1);
    return date;
  };

// Use the helper function to adjust the time

  if (!availableTimes.length && selectedTime != null) {
    return <div className={classes.timePicker}><p>Keine verfügbaren Zeiten</p>{selectedTime}</div>
  } else if (availableTimes.length) {
    return (
        <div className={classes.timePicker}>
          <button className={`${classes.timePickerButton} ${isOpen ? classes.timePickerButtonOpen : ''}`} onClick={() => setIsOpen(!isOpen)}><h3>{selectedTime ? new Date(selectedTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false }) : 'Zeit wählen'}</h3></button>
          {isOpen && (
              <div className={classes.timePickerDropdown}>
                {availableTimes.map((time, index) => (
                    <button className={classes.timePickerDropdownTimeButton} key={index} onClick={() => handleSelectTime(addOneHour(time.reservationTime))}>
                      <p>
                        {addOneHour(time.reservationTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false })}
                      </p>
                    </button>
                ))}
                {availableTimes.length % 2 !== 0 && <div className={classes.timePickerDropdownTimeDiv} />}
              </div>
          )}
        </div>
    );
  }
};

export default CustomTimePicker;