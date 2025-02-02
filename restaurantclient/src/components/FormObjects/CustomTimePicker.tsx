import { useState, useEffect } from "react";
import classes from "./CustomTimePicker.module.sass";
import { AvailabilityByDay } from "../../helpers/models/availability.ts";

function CustomTimePicker({ availableTimes, onSelectTime }: {
  availableTimes: AvailabilityByDay[]; // Array of time strings (e.g., ["13:00", "13:15", ...])
  onSelectTime: (time: string) => void; // Callback when a time is selected
}) {
  const [selectedTime, setSelectedTime] = useState<string | null>(null);
  const [isOpen, setIsOpen] = useState(false);

  useEffect(() => {
    console.log(selectedTime);
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

  if (!availableTimes.length) {
    return <div className={classes.timePicker}>Keine verfügbaren Zeiten</div>
  } else {
    return (
        <div className={classes.timePicker}>
          <button onClick={() => setIsOpen(!isOpen)}><h3>{selectedTime ? new Date(selectedTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false }) : 'Choose Time'}</h3></button>
          {isOpen && (
              <div className={classes.timePickerDropdown}>
                {availableTimes.map((time, index) => (
                    <button key={index} onClick={() => handleSelectTime(new Date(time.reservationTime))}>
                      <h2>
                        {new Date(time.reservationTime).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false })}
                      </h2>
                    </button>
                ))}
              </div>
          )}
        </div>
    );
  }
};

export default CustomTimePicker;