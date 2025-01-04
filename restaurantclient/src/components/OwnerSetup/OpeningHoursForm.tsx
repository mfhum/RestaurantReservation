import classes from './OpeningHoursForm.module.sass';
import React, { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { getAllOpeningHours } from '../../helpers/api/openingHoursApi.ts';
import { OpeningHoursObject } from '../../helpers/models/openinghours.ts';

function OpeningHoursForm() {
  const days = [0, 1, 2, 3, 4, 5, 6, 7];
  const dayMap = [99, undefined, undefined, undefined, undefined, undefined, undefined, undefined];
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

  const handleOpeningHoursFormSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const newOpeningHours: OpeningHoursObject = {
      day: weekday,
      openingTime: formValues.openingTime,
      breakStartTime: formValues.breakStartTime,
      breakEndTime: formValues.breakEndTime,
      closingTime: formValues.closingTime,
      restaurantId: "019431d9-d9f5-7463-b20d-a3e9d6badfe0"
    };
    if (!timeRegex.test(newOpeningHours.openingTime) || !timeRegex.test(newOpeningHours.closingTime)) {
      alert('Please enter a valid time');
      return;
    }
    if (newOpeningHours.breakStartTime !== '' && newOpeningHours.breakEndTime !== '') {
      if (!timeRegex.test(newOpeningHours.breakStartTime) || !timeRegex.test(newOpeningHours.breakEndTime)) {
        alert('Please enter a valid time');
        return;
      }
    }
    event.currentTarget.reset();
    setWeekday(days[0]);
    alert('Opening Hours Submitted');
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setFormValues({ ...formValues, [name]: value });
  };

  if (GetAllOpeningHours.data) {
    for (let i = 0; i < GetAllOpeningHours.data.length; i++) {
      dayMap[GetAllOpeningHours.data[i].day] = i;
    }
    console.log(dayMap);
  }

  return (
      <>
        {GetAllOpeningHours.isPending ? (
            'Loading...'
        ) : (
            <>
              <li className={classes.weekDayFormList}>
                <ul onClick={() => setWeekday(days[1])}>
                  Mo<div className={dayMap[1] != undefined ? classes.open : classes.closed}></div>
                </ul>
                <ul onClick={() => setWeekday(days[2])}>
                  Di<div className={dayMap[2] != undefined ? classes.open : classes.closed}></div>
                </ul>
                <ul onClick={() => setWeekday(days[3])}>
                  Mi<div className={dayMap[3] != undefined ? classes.open : classes.closed}></div>
                </ul>
                <ul onClick={() => setWeekday(days[4])}>
                  Do<div className={dayMap[4] != undefined ? classes.open : classes.closed}></div>
                </ul>
                <ul onClick={() => setWeekday(days[5])}>
                  Fr<div className={dayMap[5] != undefined ? classes.open : classes.closed}></div>
                </ul>
                <ul onClick={() => setWeekday(days[6])}>
                  Sa<div className={dayMap[6] != undefined ? classes.open : classes.closed}></div>
                </ul>
                <ul onClick={() => setWeekday(days[7])}>
                  So<div className={dayMap[7] != undefined ? classes.open : classes.closed}></div>
                </ul>
              </li>
              {weekday !== days[0] && (
                  <>
                    <h2>{weekday}</h2>
                    <form className={classes.openingHoursForm} onSubmit={handleOpeningHoursFormSubmit}>
                      <label htmlFor="openingTime">Opening Time*</label>
                      <input
                          type="text"
                          id="openingTime"
                          name="openingTime"
                          placeholder="10:00"
                          value={formValues.openingTime}
                          onChange={handleInputChange}
                          required
                      />
                      <label htmlFor="breakStartTime">Break Start Time</label>
                      <input
                          type="text"
                          id="breakStartTime"
                          name="breakStartTime"
                          placeholder="10:00"
                          value={formValues.breakStartTime}
                          onChange={handleInputChange}
                      />
                      <label htmlFor="breakEndTime">Break End Time</label>
                      <input
                          type="text"
                          id="breakEndTime"
                          name="breakEndTime"
                          placeholder="10:00"
                          value={formValues.breakEndTime}
                          onChange={handleInputChange}
                      />
                      <label htmlFor="closingTime">Closing Time*</label>
                      <input
                          type="text"
                          id="closingTime"
                          name="closingTime"
                          placeholder="10:00"
                          value={formValues.closingTime}
                          onChange={handleInputChange}
                          required
                      />
                      <br />
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