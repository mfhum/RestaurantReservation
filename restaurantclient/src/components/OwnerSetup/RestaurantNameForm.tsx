import React from "react";
import {RestaurantObject} from "../../helpers/models/restaurant.ts";
import {useMutation} from "@tanstack/react-query";
import {createRestaurant} from "../../helpers/api/restaurantApi.ts";

function RestaurantNameForm() {

  const CreateRestaurantQuery = useMutation<RestaurantObject, Error, RestaurantObject>({
    mutationFn: createRestaurant,
    onSuccess: () => {
      // Do something after the mutation has succeeded
    },
  });


  const handleRestaurantNameFormSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    // Prevent the default form submission behavior
    event.preventDefault();
    const restaurantName = event.currentTarget.restaurantName.value;
    const creatableRestaurant: RestaurantObject = {
      name: restaurantName
    }
    CreateRestaurantQuery.mutate(creatableRestaurant);
  }
  return (
      <>
        <h1>OwnerSetup</h1>
        {CreateRestaurantQuery.isPending && (
            <>
              <form onSubmit={handleRestaurantNameFormSubmit}>
                <label htmlFor="restaurantName">Restaurant Name:</label>
                <input type="text" id="restaurantName" name="restaurantName" required/>
                <button type="submit" disabled={CreateRestaurantQuery.data != null}>Submit</button>
              </form>
              {CreateRestaurantQuery.isPending && 'Enter Data...'}
            </>
        )};
      </>

  );
}

export default RestaurantNameForm;