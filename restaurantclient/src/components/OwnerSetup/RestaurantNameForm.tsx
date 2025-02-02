import React from "react";
import { RestaurantObject } from "../../helpers/models/restaurant.ts";
import { useMutation } from "@tanstack/react-query";
import { createRestaurant } from "../../helpers/api/restaurantApi.ts";
import classes from "./RestaurantNameForm.module.sass";

function RestaurantNameForm({onComplete}: {onComplete: () => void}) {
  const CreateRestaurantQuery = useMutation<RestaurantObject, Error, RestaurantObject>({
    mutationFn: createRestaurant,
    onSuccess: () => {
      onComplete();
    },
  });

  const handleRestaurantNameFormSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const restaurantName = event.currentTarget.restaurantName.value;
    const creatableRestaurant: RestaurantObject = {
      name: restaurantName,
    };
    CreateRestaurantQuery.mutate(creatableRestaurant);
  };

  return (
      <div className={classes.restaurantFormContainer}>
        {CreateRestaurantQuery.isPending ? (
            <h2 className={classes.loadingText}>Restaurant wird erstellt...</h2>
        ) : (
            <form onSubmit={handleRestaurantNameFormSubmit} className={classes.restaurantForm}>
              <label htmlFor="restaurantName" className={classes.restaurantLabel}>
                <h3>Restaurant Name:</h3>
              </label>
              <input type="text" id="restaurantName" name="restaurantName" required className={classes.restaurantInput} />
              <button type="submit" disabled={CreateRestaurantQuery.data != null} className={classes.restaurantSubmit}>
                Erstellen
              </button>
            </form>
        )}
      </div>
  );
}

export default RestaurantNameForm;