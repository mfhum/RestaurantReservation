import classes from './ReservationPlatform.module.sass';

function ReservationPlatform() {
  return (
      <>
        <section className={classes.reservationPlatform}>
          <form>
            <label htmlFor="guests">Amount of guests:</label>
            <input type="number" id="guests" name="guests" min="1" max="10" required/>
            <label htmlFor="date">Date:</label>
            <input type="date" id="date" name="date" required/>
            <label htmlFor="time">Time:</label>
            <input type="time" id="time" name="time" required/>
            <label htmlFor="notes">Notes:</label>
            <textarea id="notes" name="notes" placeholder="Any special requests?"></textarea>
            <button type="submit">Submit</button>
          </form>
        </section>
      </>
  )
}

export default ReservationPlatform
