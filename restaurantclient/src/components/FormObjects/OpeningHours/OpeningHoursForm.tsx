import classes from './OpeningHoursForm.module.sass';
import React, { useEffect, useState } from 'react';
import { useMutation, useQuery } from '@tanstack/react-query';
import { createOpeningHours, getAllOpeningHours } from '../../../api/requests/openingHoursApi.ts';
import { OpeningHoursObject } from '../../../api/models/openinghours.ts';

function OpeningHoursForm() {
  const days = [0, 1, 2, 3, 4, 5, 6, 7];
  const dayMapBase = [99, undefined, undefined, undefined, undefined, undefined, undefined, undefined];
  const [dayMap, setDayMap] = useState(dayMapBase);
  const [weekday, setWeekday] = useState(days[0]);
  const [formValues, setFormValues] = useState({
    openingTime: '',
    breakStartTime: '',
    breakEndTime: '',
    closingTime: ''
  });
  const [successMessage, setSuccessMessage] = useState<string | null>(null);
  const timeRegex = /^([01]?\d|2[0-3]):([0-5]\d)$/; // Allow 0-23 hours and 00-59 minutes
  getAllOpeningHours
  const GetAllOpeningHours = useQuery({
    queryKey: ['GetAllOpeningHours'],
    queryFn: () => getAllOpeningHours(),
    enabled: true,
  });

  const CreateOpeningHours = useMutation<OpeningHoursObject, Error, OpeningHoursObject>({
    mutationFn: createOpeningHours,
    onSuccess: () => {
      GetAllOpeningHours.refetch();
      setSuccessMessage('Änderungen erfolgreich gespeichert.');
      setTimeout(() => setSuccessMessage(null), 3000); // Hide message after 3 seconds
    },
  });

  const formatTime = (time: string | undefined) => {
    if (!time) return ''; // Return empty if undefined
    const daySplit = time.split('.');
    const timePart = daySplit.length === 2 ? daySplit[1] : daySplit[0]; // Extract "03:00:00"
    const parts = timePart.split(':');
    return `${parts[0]}:${parts[1]}`; // Return only hh:mm
  };

  useEffect(() => {
    if (GetAllOpeningHours.data) {
      const updatedDayMap: (OpeningHoursObject | undefined)[] = Array.from({ length: 8 }, () => undefined);
      GetAllOpeningHours.data.forEach((item) => {
        updatedDayMap[item.day] = item;
      });

      const arraysAreEqual = (a: (OpeningHoursObject | undefined)[], b: (OpeningHoursObject | undefined)[]) => {
        if (a.length !== b.length) return false;
        for (let i = 0; i < a.length; i++) {
          if (JSON.stringify(a[i]) !== JSON.stringify(b[i])) {
            return false;
          }
        }
        return true;
      };

      // @ts-expect-error: this works dayMap is mapped correctly
      if (!arraysAreEqual(updatedDayMap, dayMap)) {
        // @ts-expect-error: Check if dayMap has changed
        setDayMap(updatedDayMap);
      }
    }
  }, [GetAllOpeningHours.data, dayMap]);

  const handleDayClick = (day: number) => {
    setWeekday(day);
    const existingData = dayMap[day];
    setFormValues({
      // @ts-expect-error: dayMap is mapped correctly
      openingTime: addOneHour(formatTime(existingData?.openingTime)),
      // @ts-expect-error: dayMap is mapped correctly
      breakStartTime: addOneHour(formatTime(existingData?.breakStartTime)),
      // @ts-expect-error: dayMap is mapped correctly
      breakEndTime: addOneHour(formatTime(existingData?.breakEndTime)),
      // @ts-expect-error: dayMap is mapped correctly
      closingTime: addOneHour(formatTime(existingData?.closingTime))
    });
  };
  const subtractOneHour = (time: string) => {
    const [hours, minutes] = time.split(':').map(Number);
    const date = new Date();
    date.setHours(hours - 1, minutes);
    return date.toTimeString().slice(0, 5);
  };
  const addOneHour = (time: string) => {
    if (!time) return '';
    const [hours, minutes] = time.split(':').map(Number);
    const date = new Date();
    date.setHours(hours + 1, minutes);
    return date.toTimeString().slice(0, 5);
  };

  
  const handleOpeningHoursFormSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const newOpeningHours: OpeningHoursObject = {
      day: weekday,
      openingTime: subtractOneHour(formValues.openingTime),
      breakStartTime: formValues.breakStartTime ? subtractOneHour(formValues.breakStartTime) : undefined,
      breakEndTime: formValues.breakEndTime ? subtractOneHour(formValues.breakEndTime) : undefined,
      closingTime: subtractOneHour(formValues.closingTime)
    };
    if (!timeRegex.test(newOpeningHours.openingTime) || !timeRegex.test(newOpeningHours.closingTime)) {
      alert('Please enter a valid opening and closing time in HH:mm format.');
      return;
    }

    if (
        (newOpeningHours.breakStartTime && !timeRegex.test(newOpeningHours.breakStartTime)) ||
        (newOpeningHours.breakEndTime && !timeRegex.test(newOpeningHours.breakEndTime))
    ) {
      alert('Please enter valid break times in HH:mm format.');
      return;
    }
    CreateOpeningHours.mutate(newOpeningHours);
    event.currentTarget.reset();
    setWeekday(days[0]);
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    const formattedValue = value.length === 4 && !value.includes(':') ? `${value.slice(0, 2)}:${value.slice(2)}` : value;
    setFormValues({ ...formValues, [name]: formattedValue });
  };

  return (
      <>
        {GetAllOpeningHours.isLoading ? (
            'Loading...'
        ) : (
            <>
              <li className={classes.weekDayFormList}>
                {days.slice(1).map((day, index) => (
                    <ul key={index} onClick={() => handleDayClick(day)}>
                      <h3>{['Mo', 'Di', 'Mi', 'Do', 'Fr', 'Sa', 'So'][index]}</h3>
                      <div className={dayMap[day] ? classes.open : classes.closed}></div>
                    </ul>
                ))}
              </li>
              <div className={classes.openingHoursFormContainer}>
                {weekday !== days[0] && (
                    <>
                      <h2>{['Montag', 'Dienstag', 'Mittwoch', 'Donnerstag', 'Freitag', 'Samstag', 'Sonntag'][weekday - 1]}</h2>
                      <form className={classes.openingHoursForm} onSubmit={handleOpeningHoursFormSubmit}>
                        <label htmlFor="openingTime"><p>Opening Time*</p></label>
                        <input
                            type="text"
                            id="openingTime"
                            name="openingTime"
                            value={formValues.openingTime}
                            pattern="^([01]?\d|2[0-3]):([0-5]\d)$"
                            placeholder="HH:mm"
                            onChange={handleInputChange}
                            required
                        />
                        <label htmlFor="breakStartTime"><p>Break Start Time</p></label>
                        <input
                            type="text"
                            id="breakStartTime"
                            name="breakStartTime"
                            value={formValues.breakStartTime}
                            pattern="^([01]?\d|2[0-3]):([0-5]\d)$"
                            placeholder="HH:mm"
                            onChange={handleInputChange}
                        />
                        <label htmlFor="breakEndTime"><p>Break End Time</p></label>
                        <input
                            type="text"
                            id="breakEndTime"
                            name="breakEndTime"
                            value={formValues.breakEndTime}
                            pattern="^([01]?\d|2[0-3]):([0-5]\d)$"
                            placeholder="HH:mm"
                            onChange={handleInputChange}
                        />
                        <label htmlFor="closingTime"><p>Closing Time*</p></label>
                        <input
                            type="text"
                            id="closingTime"
                            name="closingTime"
                            value={formValues.closingTime}
                            pattern="^([01]?\d|2[0-3]):([0-5]\d)$"
                            placeholder="HH:mm"
                            onChange={handleInputChange}
                            required
                        />
                        <button className={classes.submitButton} type="submit"><h3>Erstellen</h3></button>
                      </form>
                    </>

                )}
                {successMessage && <p className={classes.successMessage}>{successMessage}</p>}
              </div>

            </>
        )}
      </>
  );
}

export default OpeningHoursForm;