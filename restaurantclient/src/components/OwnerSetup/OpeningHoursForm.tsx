import classes from './OpeningHoursForm.module.sass';
import React, { useEffect, useState } from 'react';
import { useMutation, useQuery } from '@tanstack/react-query';
import { createOpeningHours, getAllOpeningHours } from '../../helpers/api/openingHoursApi.ts';
import { OpeningHoursObject } from '../../helpers/models/openinghours.ts';

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
  const timeRegex = /^([01]\d|2[0-3]):([0-5]\d)$/;

  const GetAllOpeningHours = useQuery({
    queryKey: ['GetAllOpeningHours'],
    queryFn: () => getAllOpeningHours(),
    enabled: true,
  });

  const CreateOpeningHours = useMutation<OpeningHoursObject, Error, OpeningHoursObject>({
    mutationFn: createOpeningHours, // Pass the mutation function
    onSuccess: () => {
      GetAllOpeningHours.refetch(); // Trigger a refetch on success
    },
  });

  // Handle form submission
  const handleOpeningHoursFormSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    console.log(formValues);
    const newOpeningHours: OpeningHoursObject = {
      day: weekday,
      openingTime: formValues.openingTime,
      breakStartTime: formValues.breakStartTime != '' ? formValues.breakStartTime : undefined,
      breakEndTime: formValues.breakEndTime != '' ? formValues.breakEndTime : undefined,
      closingTime: formValues.closingTime,
      restaurantId: "019431d9-d9f5-7463-b20d-a3e9d6badfe0"
    };
    if (!timeRegex.test(newOpeningHours.openingTime) || !timeRegex.test(newOpeningHours.closingTime)) {
      alert('Please enter a valid time');
      return;
    }
    if (newOpeningHours.breakStartTime !== undefined && newOpeningHours.breakEndTime !== undefined) {
      if (!timeRegex.test(newOpeningHours.breakStartTime as string) || !timeRegex.test(newOpeningHours.breakEndTime as string)) {
        alert('Please enter a valid time');
        return;
      }
    }
    event.currentTarget.reset();
    setWeekday(days[0]);
    console.log(newOpeningHours);
    CreateOpeningHours.mutate(newOpeningHours);
    alert('Opening Hours Submitted');
  };

  // Handle input change
  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setFormValues({ ...formValues, [name]: value });
  };

  // Populate dayMap on data fetch
  useEffect(() => {
    if (GetAllOpeningHours.data) {
      const updatedDayMap: OpeningHoursObject[] = GetAllOpeningHours.data.reduce<OpeningHoursObject[]>(
          (map, item) => {
            map[item.day] = item; // Map the day to the corresponding object
            return map;
          },
          Array.from({ length: 8 }, () => undefined) // Safely initialize array
      );

      const arraysAreEqual = (a: any[], b: any[]) => {
        return a.length === b.length && a.every((val, index) => val === b[index]);
      };

      if (!arraysAreEqual(updatedDayMap, dayMap)) {
        setDayMap(updatedDayMap);
      }
    }
  }, [GetAllOpeningHours.data]);

  // Handle day selection and populate form with existing data
  const handleDayClick = (day: number) => {
    setWeekday(day);
    const existingData = dayMap[day];
    if (existingData) {
      setFormValues({
        openingTime: existingData.openingTime ? existingData.openingTime.slice(0, 5) : '', // Format to HH:mm
        breakStartTime: existingData.breakStartTime ? existingData.breakStartTime.slice(0, 5) : '',
        breakEndTime: existingData.breakEndTime ? existingData.breakEndTime.slice(0, 5) : '',
        closingTime: existingData.closingTime ? existingData.closingTime.slice(0, 5) : ''
      });
    } else {
      setFormValues({
        openingTime: '',
        breakStartTime: '',
        breakEndTime: '',
        closingTime: ''
      });
    }
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
                      {['Mo', 'Di', 'Mi', 'Do', 'Fr', 'Sa', 'So'][index]}
                      <div className={dayMap[day] != undefined ? classes.open : classes.closed}></div>
                    </ul>
                ))}
              </li>
              {weekday !== days[0] && (
                  <>
                    <h2>{['Mo', 'Di', 'Mi', 'Do', 'Fr', 'Sa', 'So'][weekday - 1]}</h2>
                    <form className={classes.openingHoursForm} onSubmit={handleOpeningHoursFormSubmit}>
                      <label htmlFor="openingTime">Opening Time*</label>
                      <input
                          type="text"
                          id="openingTime"
                          name="openingTime"
                          value={formValues.openingTime}
                          pattern="^([01]\d|2[0-3]):([0-5]\d)$"
                          placeholder={'HH:mm'}
                          onChange={handleInputChange}
                          required
                      />
                      <label htmlFor="breakStartTime">Break Start Time</label>
                      <input
                          type="text"
                          id="breakStartTime"
                          name="breakStartTime"
                          value={formValues.breakStartTime}
                          pattern="^([01]\d|2[0-3]):([0-5]\d)$"
                          placeholder={'HH:mm'}
                          onChange={handleInputChange}
                      />
                      <label htmlFor="breakEndTime">Break End Time</label>
                      <input
                          type="text"
                          id="breakEndTime"
                          name="breakEndTime"
                          value={formValues.breakEndTime}
                          pattern="^([01]\d|2[0-3]):([0-5]\d)$"
                          placeholder={'HH:mm'}
                          onChange={handleInputChange}
                      />
                      <label htmlFor="closingTime">Closing Time*</label>
                      <input
                          type="text"
                          id="closingTime"
                          name="closingTime"
                          value={formValues.closingTime}
                          pattern="^([01]\d|2[0-3]):([0-5]\d)$"
                          placeholder={'HH:mm'}
                          onChange={handleInputChange}
                          required
                      />
                      <br/>
                      <button type="submit">Submit</button>
                    </form>
                  </>
              )}
            </>
        )}
      </>
  );
}

export default OpeningHoursForm;
