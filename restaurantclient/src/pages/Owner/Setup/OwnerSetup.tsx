import { useState } from "react";
import classes from "./OwnerSetup.module.sass";
import RestaurantNameForm from "../../../components/OwnerSetup/RestaurantNameForm.tsx";
import OpeningHoursForm from "../../../components/OwnerSetup/OpeningHoursForm.tsx";
import Tables from "../../../components/CreateTables/tablesForm.tsx"; // Import CSS Module as 'classes'

const OwnerSetup = () => {
    const [restaurantFormDone, setRestaurantFormDone] = useState(false);
    const [openingHoursFormStart, setOpeningHoursFormStart] = useState(false);
    const [openingHoursFormDone, setOpeningHoursFormDone] = useState(false);
    const [tablesFormStart, setTablesFormStart] = useState(false);
    const [tablesFormDone, setTablesFormDone] = useState(false);

    const handleRestaurantNameComplete = () => {
        setRestaurantFormDone(true);
    };

    return (
        <div className={classes.ownerSetup}>
            <h1 className={classes.title}>Restaurant Reservation Tool</h1>
            {tablesFormDone ? <h2 className={classes.subtitle}>Einrichtung abgeschlossen!</h2> : <h2 className={classes.subtitle}>Lass uns damit beginnen, dein Restaurant einzurichten.</h2>}
            {!tablesFormDone ? (
                <>
                    <div className={classes.formsContainer}>
                        {!restaurantFormDone ? (
                            <div className={`${restaurantFormDone ? classes.slideOut : ""}`}>
                                <RestaurantNameForm onComplete={handleRestaurantNameComplete} />
                            </div>
                        ) : (
                            <>
                                {!openingHoursFormStart ? (
                                    <>
                                        <h3 className={classes.message}>Super, nun können wir mit den Öffnungszeiten weiter machen!</h3>
                                        <button onClick={() => setOpeningHoursFormStart(true)} className={classes.button}><h3>Öffnungszeiten erstellen</h3></button>
                                    </>
                                ) : (
                                    <>
                                        {!openingHoursFormDone ? (
                                            <>
                                                <OpeningHoursForm/>
                                                <button onClick={() => setOpeningHoursFormDone(true)} className={classes.button}><h3>Zum nächsten Schritt</h3></button>
                                            </>
                                        ) : (
                                            <>
                                                {!tablesFormStart ? (
                                                    <>
                                                        <h2 className={classes.message}>Öffnungszeiten wurden erstellt!</h2>
                                                        <button onClick={() => setTablesFormStart(true)} className={classes.button}><h3>Tische erstellen</h3></button>
                                                    </>
                                                ):(
                                                    <>
                                                        <Tables/>
                                                        <button onClick={() => setTablesFormDone(true)} className={classes.button}><h3>Einrichtung abschliessen</h3></button>

                                                    </>
                                                )}

                                            </>
                                        )}
                                    </>
                                )}
                            </>
                        )}
                    </div>
                    {openingHoursFormStart && !openingHoursFormDone && (
                        <div className={classes.infoText}>
                            <h3>Klicke auf einen Tag!</h3>
                            <p>Danach kannst du Öffnungszeiten und Pausen eingeben für den jeweiligen Tag</p>
                            <p>Grün = Restaurant geöffnet, Rot = Restaurant geschlossen</p>
                        </div>
                    )}
                    {tablesFormStart && !tablesFormDone && (
                        <div className={classes.infoText}>
                            <h3>Erstelle einen neuen Tisch!</h3>
                            <p>Rechts siehst du bereits vorhandene Tische.</p>
                            <p>Die Tischnummer muss einzigartig sein.</p>
                            <p>Hast du alle erstellt, kannst du den Vorgang abschliessen!</p>
                        </div>
                    )}
                </>
            ) : (
                <>
                    <h3>Super!</h3>
                    <h3>Das Restaurant ist nun vorbereitet für deine ersten Reservationen.</h3>
                    <button className={classes.button}><h3>Reservation erstellen</h3></button>
                </>
                
            )}
            
            
           
        </div>
    );
};

export default OwnerSetup;