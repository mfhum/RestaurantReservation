import { useState } from "react";
import classes from "./CustomTimePicker.module.sass";
import {AvailabilityByDay} from "../../helpers/models/availability.ts";

function CustomTimePicker({ availableTimes, onSelectTime }: {
  availableTimes: AvailabilityByDay[]; // Array of time strings (e.g., ["13:00", "13:15", ...])
  onSelectTime: (time: string) => void; // Callback when a time is selected
}) {
  console.log("time:", availableTimes);
  const [selectedTime, setSelectedTime] = useState<string | null>(null);
  const [isOpen, setIsOpen] = useState(false);

  const handleSelectTime = (time: string) => {
    setSelectedTime(time);
    onSelectTime(time);
    setIsOpen(false); // Close the dropdown after selecting
  };

  if (!availableTimes.length) {
    return <div className={classes.timePicker}>Keine verfügbaren Zeiten</div>
  } else {
    return (
        <div className={classes.timePicker}>
          {availableTimes.map((time, index) => (
              <div key={index}>
                {time.reservationTime}
              </div>
          ))}

        </div>
    );
  }
};

export default CustomTimePicker;
