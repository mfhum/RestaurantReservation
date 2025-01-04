import classes from './OwnerSetup.module.sass';
import OpeningHoursForm from "../../../components/OwnerSetup/OpeningHoursForm.tsx";

function OwnerSetup() {



  return (
      <>
        <section className={classes.ownerSetup}>
          <OpeningHoursForm/>
        </section>
      </>
  )
}

export default OwnerSetup
