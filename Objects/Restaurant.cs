using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CuisineProject
{
  public class Restaurant
  {
    private int _id;
    private int _cuisineId;
    private string _name;
    private string _city;
    private int _rating;

    public Restaurant(string name, int cuisineId, string city, int rating, int restaurantId = 0)
    {
      _id = restaurantId;
      _cuisineId = cuisineId;
      _name = name;
      _city = city;
      _rating = rating;
    }

    public int GetId()
    {
      return _id;
    }
    public int GetCuisineId()
    {
      return _cuisineId;
    }
    public string GetName()
    {
      return _name;
    }
    public string GetCity()
    {
      return _city;
    }
    public int GetRating()
    {
      return _rating;
    }


    public override bool Equals(System.Object otherRestaurant)
    {
      if (!(otherRestaurant is Restaurant))
      {
        return false;
      }
      else
      {
        Restaurant newRestaurant = (Restaurant) otherRestaurant;
        bool idEquality = (this.GetId() == newRestaurant.GetId());
        bool nameEquality = (this.GetName() == newRestaurant.GetName());
        bool cityEquality = (this.GetCity() == newRestaurant.GetCity());
        bool ratingEquality = (this.GetRating() == newRestaurant.GetRating());
        bool cuisineIdEquality = (this.GetCuisineId() == newRestaurant.GetCuisineId());


        return (idEquality && nameEquality && cityEquality && ratingEquality && cuisineIdEquality);
      }
    }

    public static List<Restaurant> GetAll()
    {
      List<Restaurant> allRestaurants = new List<Restaurant>();

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int restaurantId = rdr.GetInt32(0);
        string restaurantName = rdr.GetString(1);
        int restaurantCuisineId = rdr.GetInt32(2);
        string restaurantCity = rdr.GetString(3);
        int restaurantRating = rdr.GetInt32(4);

        Restaurant newRestaurant = new Restaurant(restaurantName, restaurantCuisineId, restaurantCity, restaurantRating, restaurantId);
        allRestaurants.Add(newRestaurant);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allRestaurants;
    }


    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO restaurants (name, cuisine_id, city, rating) OUTPUT INSERTED.id VALUES (@Name, @CuisineId, @City, @Rating);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@Name";
      nameParameter.Value = this.GetName();

      SqlParameter cuisineIdParameter = new SqlParameter();
      cuisineIdParameter.ParameterName = "@CuisineId";
      cuisineIdParameter.Value = this.GetCuisineId();

      SqlParameter cityParameter = new SqlParameter();
      cityParameter.ParameterName = "@City";
      cityParameter.Value = this.GetCity();

      SqlParameter ratingParameter = new SqlParameter();
      ratingParameter.ParameterName = "@Rating";
      ratingParameter.Value = this.GetRating();


      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(cuisineIdParameter);
      cmd.Parameters.Add(cityParameter);
      cmd.Parameters.Add(ratingParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public static List<Restaurant> GetAllByType(int typeId)
    {
      List<Restaurant> getRestaurants = new List<Restaurant>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants WHERE cuisine_id = @Id;", conn);
      SqlParameter restaurantIdParameter = new SqlParameter();
      restaurantIdParameter.ParameterName = "@Id";
      restaurantIdParameter.Value = typeId.ToString();
      cmd.Parameters.Add(restaurantIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int restaurantId = rdr.GetInt32(0);
        string restuarantName = rdr.GetString(1);
        int cuisineId = rdr.GetInt32(2);
        string restaurantCity = rdr.GetString(3);
        int ratingId = rdr.GetInt32(4);

        Restaurant newRestaurant = new Restaurant(restuarantName, cuisineId, restaurantCity, ratingId, restaurantId);
        getRestaurants.Add(newRestaurant);
      }

        if (rdr != null)
        {
         rdr.Close();
       }
       if (conn != null)
       {
           conn.Close();
       }

      return getRestaurants;
    }

    public static Restaurant Find(int searchId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM restaurants WHERE id = @searchId;", conn);
      SqlParameter restaurantIdParameter = new SqlParameter();
      restaurantIdParameter.ParameterName = "@searchId";
      restaurantIdParameter.Value = searchId.ToString();
      cmd.Parameters.Add(restaurantIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int restaurantId = 0;
      string restaurantName = null;
      int restaurantCuisineId = 0;
      string restaurantCity = null;
      int restaurantRating = 0;

      while(rdr.Read())
      {
        restaurantId = rdr.GetInt32(0);
        restaurantName = rdr.GetString(1);
        restaurantCuisineId = rdr.GetInt32(2);
        restaurantCity = rdr.GetString(3);
        restaurantRating = rdr.GetInt32(4);
      }
      Restaurant newRestaurant = new Restaurant(restaurantName, restaurantCuisineId, restaurantCity, restaurantRating, restaurantId);
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return newRestaurant;
    }

    /////////////////////////////// USE THIS THINGY ///////////////////////////////
    public void Update(string newName, string newCity, int newRating)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlParameter newNameParameter = new SqlParameter("@NewName", newName);
      SqlParameter newCityParameter = new SqlParameter("@NewCity", newCity);
      SqlParameter newRatingParameter = new SqlParameter("@NewRating", newRating);
      SqlParameter newIdParameter = new SqlParameter("@Id", this.GetId());

      SqlCommand cmd = new SqlCommand("UPDATE restaurants SET name = @NewName, city = @NewCity, rating = @NewRating OUTPUT INSERTED.name, INSERTED.city, INSERTED.rating WHERE id = @Id;", conn);

      cmd.Parameters.Add(newNameParameter);
      cmd.Parameters.Add(newCityParameter);
      cmd.Parameters.Add(newRatingParameter);
      cmd.Parameters.Add(newIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._name = rdr.GetString(0);
        this._city = rdr.GetString(1);
        this._rating = rdr.GetInt32(2);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }


    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM restaurants WHERE id = @Id;", conn);

      SqlParameter restaurantIdParameter = new SqlParameter();
      restaurantIdParameter.ParameterName = "@Id";
      restaurantIdParameter.Value = this.GetId();

      cmd.Parameters.Add(restaurantIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM restaurants;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
