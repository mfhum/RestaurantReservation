import React, {useState} from "react";
import {RestaurantObject} from "../../helpers/models/restaurant.ts";
import {useQuery} from "@tanstack/react-query";
import {createRestaurant} from "../../helpers/api/restaurantApi.ts";

function RestaurantNameForm() {
  const [newRestaurant, setNewRestaurant] = useState<RestaurantObject>({ name: '' });
  const CreateRestaurantQuery= useQuery({
    queryKey: ['CreateRestaurant', newRestaurant],
    queryFn: () => createRestaurant(newRestaurant),
    enabled: newRestaurant.name != '',
  })

  const handleRestaurantNameFormSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    // Prevent the default form submission behavior
    event.preventDefault();
    const restaurantName = event.currentTarget.restaurantName.value;
    const creatableRestaurant: RestaurantObject = {
      name: restaurantName
    }
    setNewRestaurant(creatableRestaurant);
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