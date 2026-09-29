using Domain.Entities;

namespace Infrastructure.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        context.Database.EnsureCreated();

        SeedCities(context);
        SeedRestaurants(context);
        SeedMenuItems(context);
    }

    private static void SeedCities(AppDbContext context)
    {
        if (context.Cities.Any())
        {
            return;
        }

        var cities = new List<City>
        {
            new()
            {
                Id = 1,
                Name = "Cairo"
            },
            new()
            {
                Id = 2,
                Name = "Giza"
            },
            new()
            {
                Id = 3,
                Name = "Alexandria"
            }
        };

        context.Cities.AddRange(cities);

        context.SaveChanges();
    }

    private static void SeedRestaurants(
        AppDbContext context)
    {
        if (context.Restaurants.Count() >= 30)
        {
            return;
        }

        var restaurants = new List<Restaurant>
        {
            // =========================
            // CAIRO
            // =========================

            new()
            {
                Id = 1,
                Name = "La Tavola",
                CityId = 1,
                Category = "Italian · Fine Dining",
                Description =
                    "Authentic Italian dishes prepared with fresh ingredients and served in a warm, elegant atmosphere.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.8m,
                Likes = 412,
                Comments = 248
            },

            new()
            {
                Id = 2,
                Name = "Cairo Grill House",
                CityId = 1,
                Category = "Egyptian · Grill",
                Description =
                    "Traditional Egyptian grilled meats, fresh salads and delicious local specialties.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1544025162-d76694265947?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.7m,
                Likes = 321,
                Comments = 58
            },

            new()
            {
                Id = 3,
                Name = "Nile Kitchen",
                CityId = 1,
                Category = "Egyptian · Traditional",
                Description =
                    "Classic Egyptian comfort food prepared with authentic recipes and fresh local ingredients.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1547592180-85f173990554?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.6m,
                Likes = 289,
                Comments = 51
            },

            new()
            {
                Id = 4,
                Name = "Urban Bites",
                CityId = 1,
                Category = "International · Casual",
                Description =
                    "Modern comfort food, creative sandwiches and satisfying dishes in a relaxed urban setting.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1504674900247-0877df9cc836?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.5m,
                Likes = 245,
                Comments = 44
            },

            new()
            {
                Id = 5,
                Name = "Bella Pizza",
                CityId = 1,
                Category = "Pizza · Italian",
                Description =
                    "Wood-fired pizzas topped with fresh ingredients and homemade Italian sauces.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.5m,
                Likes = 276,
                Comments = 49
            },

            new()
            {
                Id = 6,
                Name = "Sushi Garden",
                CityId = 1,
                Category = "Japanese · Sushi",
                Description =
                    "Fresh sushi rolls, sashimi and Japanese specialties prepared by experienced chefs.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1579871494447-9811cf80d66c?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.8m,
                Likes = 398,
                Comments = 72
            },

            new()
            {
                Id = 7,
                Name = "Spice Route",
                CityId = 1,
                Category = "Indian · Asian",
                Description =
                    "Rich Indian curries, aromatic rice dishes and freshly baked breads full of flavor.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1601050690597-df0568f70950?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.6m,
                Likes = 233,
                Comments = 41
            },

            new()
            {
                Id = 8,
                Name = "Fresh Bowl",
                CityId = 1,
                Category = "Healthy · Bowls",
                Description =
                    "Fresh salads, grain bowls and nutritious meals made from carefully selected ingredients.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.7m,
                Likes = 287,
                Comments = 53
            },

            new()
            {
                Id = 9,
                Name = "The Pasta Room",
                CityId = 1,
                Category = "Italian · Pasta",
                Description =
                    "Handcrafted pasta dishes with rich sauces and authentic Italian flavors.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1473093295043-cdd812d0e601?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.7m,
                Likes = 304,
                Comments = 57
            },

            new()
            {
                Id = 10,
                Name = "Sweet Corner",
                CityId = 1,
                Category = "Desserts · Bakery",
                Description =
                    "Freshly baked cakes, pastries and sweet treats prepared throughout the day.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1551024506-0bccd828d307?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.6m,
                Likes = 219,
                Comments = 38
            },

            // =========================
            // GIZA
            // =========================

            new()
            {
                Id = 11,
                Name = "Pyramid Kitchen",
                CityId = 2,
                Category = "Egyptian · Grill",
                Description =
                    "A modern Egyptian kitchen serving grilled favorites and traditional specialties.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1544025162-d76694265947?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.8m,
                Likes = 376,
                Comments = 67
            },

            new()
            {
                Id = 12,
                Name = "Garden Grill",
                CityId = 2,
                Category = "Grill · International",
                Description =
                    "Fresh grilled dishes served with vegetables, sauces and signature sides.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1555939594-58d7cb561ad1?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.7m,
                Likes = 351,
                Comments = 62
            },

            new()
            {
                Id = 13,
                Name = "Burger Lab",
                CityId = 2,
                Category = "Burgers · Fast Food",
                Description =
                    "Creative burgers, crispy fries and fresh sauces made for serious burger lovers.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.5m,
                Likes = 218,
                Comments = 43
            },

            new()
            {
                Id = 14,
                Name = "Mamma Mia",
                CityId = 2,
                Category = "Italian · Family",
                Description =
                    "Family-style Italian dishes with homemade sauces, pasta and oven-baked favorites.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1551183053-bf91a1d81141?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.7m,
                Likes = 315,
                Comments = 59
            },

            new()
            {
                Id = 15,
                Name = "Asian Fusion",
                CityId = 2,
                Category = "Asian · Fusion",
                Description =
                    "Modern Asian dishes combining bold flavors from Japanese, Thai and Chinese cuisine.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1512058564366-18510be2db19?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.7m,
                Likes = 267,
                Comments = 47
            },

            new()
            {
                Id = 16,
                Name = "Green Plate",
                CityId = 2,
                Category = "Healthy · Vegetarian",
                Description =
                    "Colorful healthy meals prepared with fresh vegetables and nutritious ingredients.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.6m,
                Likes = 248,
                Comments = 45
            },

            new()
            {
                Id = 17,
                Name = "Seafood Station",
                CityId = 2,
                Category = "Seafood · Casual",
                Description =
                    "Fresh seafood dishes prepared with Mediterranean flavors and house sauces.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1534939561126-855b8675edd7?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.8m,
                Likes = 372,
                Comments = 69
            },

            new()
            {
                Id = 18,
                Name = "Coffee & Cake",
                CityId = 2,
                Category = "Cafe · Desserts",
                Description =
                    "Specialty coffee, freshly baked cakes and delicious desserts for every occasion.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.6m,
                Likes = 261,
                Comments = 48
            },

            new()
            {
                Id = 19,
                Name = "Tasty Shawarma",
                CityId = 2,
                Category = "Middle Eastern · Fast Food",
                Description =
                    "Juicy shawarma, grilled meats and fresh sides served with signature sauces.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1561651823-34feb02250e4?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.7m,
                Likes = 335,
                Comments = 61
            },

            new()
            {
                Id = 20,
                Name = "Royal Breakfast",
                CityId = 2,
                Category = "Breakfast · Cafe",
                Description =
                    "Fresh breakfast plates, eggs, pastries and specialty drinks served all morning.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1533089860892-a7c6f0a88666?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.7m,
                Likes = 290,
                Comments = 52
            },

            // =========================
            // ALEXANDRIA
            // =========================

            new()
            {
                Id = 21,
                Name = "Olive Garden",
                CityId = 3,
                Category = "Mediterranean · Italian",
                Description =
                    "Mediterranean-inspired dishes made with olive oil, fresh herbs and seasonal ingredients.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1515003197210-e0cd71810b5f?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.7m,
                Likes = 315,
                Comments = 61
            },

            new()
            {
                Id = 22,
                Name = "Alex Sea View",
                CityId = 3,
                Category = "Seafood · Fine Dining",
                Description =
                    "Fresh seafood and Mediterranean specialties served with a beautiful coastal atmosphere.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1547592180-85f173990554?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.9m,
                Likes = 421,
                Comments = 83
            },

            new()
            {
                Id = 23,
                Name = "Mediterranean House",
                CityId = 3,
                Category = "Mediterranean · Family",
                Description =
                    "Traditional Mediterranean recipes combined with fresh seafood, salads and grilled specialties.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1544148103-0773bf10d330?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.8m,
                Likes = 384,
                Comments = 71
            },

            new()
            {
                Id = 24,
                Name = "Fish Market",
                CityId = 3,
                Category = "Seafood · Local",
                Description =
                    "Freshly prepared fish and seafood with simple local flavors and signature sauces.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1519708227418-c8fd9a32b7a2?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.7m,
                Likes = 363,
                Comments = 68
            },

            new()
            {
                Id = 25,
                Name = "Pasta Marina",
                CityId = 3,
                Category = "Italian · Seafood",
                Description =
                    "Italian pasta specialties inspired by the Mediterranean coast and fresh seafood.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1473093295043-cdd812d0e601?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.8m,
                Likes = 399,
                Comments = 75
            },

            new()
            {
                Id = 26,
                Name = "Sunset Cafe",
                CityId = 3,
                Category = "Cafe · International",
                Description =
                    "Relaxed seaside cafe serving coffee, light meals, sandwiches and desserts.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1501339847302-ac426a4a7cbb?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.6m,
                Likes = 231,
                Comments = 42
            },

            new()
            {
                Id = 27,
                Name = "Coastal Grill",
                CityId = 3,
                Category = "Grill · Seafood",
                Description =
                    "Charcoal grilled seafood and meats served with fresh salads and coastal sides.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1555939594-58d7cb561ad1?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.8m,
                Likes = 347,
                Comments = 65
            },

            new()
            {
                Id = 28,
                Name = "Taste of Alexandria",
                CityId = 3,
                Category = "Egyptian · Seafood",
                Description =
                    "Alexandrian favorites prepared with fresh ingredients and authentic local flavors.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1544025162-d76694265947?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.7m,
                Likes = 312,
                Comments = 58
            },

            new()
            {
                Id = 29,
                Name = "Harbor Pizza",
                CityId = 3,
                Category = "Pizza · Italian",
                Description =
                    "Crispy wood-fired pizza with fresh toppings and a beautiful harbor atmosphere.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.6m,
                Likes = 281,
                Comments = 51
            },

            new()
            {
                Id = 30,
                Name = "Blue Table",
                CityId = 3,
                Category = "International · Fine Dining",
                Description =
                    "Elegant international cuisine combining modern presentation with fresh Mediterranean flavors.",
                ImageUrl =
                    "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4?auto=format&fit=crop&w=1600&q=85",
                Rating = 4.9m,
                Likes = 401,
                Comments = 79
            }
        };

        context.Restaurants.AddRange(restaurants);

        context.SaveChanges();
    }

    private static void SeedMenuItems(
        AppDbContext context)
    {
        var restaurantIds = context.Restaurants
            .Select(x => x.Id)
            .OrderBy(x => x)
            .ToList();

        if (restaurantIds.Count == 0)
        {
            return;
        }

        var expectedCount = restaurantIds.Count * 12;

        var currentCount = context.MenuItems.Count();

        if (currentCount == expectedCount)
        {
            return;
        }

        if (currentCount > 0)
        {
            context.MenuItems.RemoveRange(
                context.MenuItems);

            context.SaveChanges();
        }

        var catalog = CreateMenuCatalog();

        var menuItems = new List<MenuItem>();

        foreach (var restaurantId in restaurantIds)
        {
            var offset =
                ((restaurantId - 1) * 7) %
                catalog.Count;

            for (var itemIndex = 0; itemIndex < 12; itemIndex++)
            {
                var catalogIndex =
                    (offset + itemIndex) % catalog.Count;

                var template = catalog[catalogIndex];

                menuItems.Add(new MenuItem
                {
                    RestaurantId = restaurantId,
                    Name = template.Name,
                    Description = template.Description,
                    Category = template.Category,
                    Price = template.Price,
                    ImageUrl = template.ImageUrl,
                    IsAvailable = true
                });
            }
        }

        context.MenuItems.AddRange(menuItems);

        context.SaveChanges();
    }

    private static List<MenuTemplate> CreateMenuCatalog()
    {
        return new List<MenuTemplate>
        {
            new(
                "Creamy Pasta",
                "Fresh pasta with creamy parmesan sauce, herbs and roasted vegetables.",
                "Pasta",
                185,
                "https://images.unsplash.com/photo-1551892374-ecf8754cf8b0?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Margherita Pizza",
                "Classic Italian pizza with tomato sauce, mozzarella and fresh basil.",
                "Pizza",
                220,
                "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Grilled Chicken",
                "Tender grilled chicken breast served with seasonal vegetables.",
                "Main Course",
                260,
                "https://images.unsplash.com/photo-1532550907401-a500c9a57435?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Beef Burger",
                "Juicy beef burger with cheddar cheese, lettuce and signature sauce.",
                "Burgers",
                235,
                "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Caesar Salad",
                "Crispy lettuce, grilled chicken, parmesan and creamy Caesar dressing.",
                "Salads",
                165,
                "https://images.unsplash.com/photo-1550304943-4f24f54ddde9?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Seafood Risotto",
                "Creamy risotto with fresh shrimp, calamari and Mediterranean herbs.",
                "Seafood",
                310,
                "https://images.unsplash.com/photo-1476124369491-e7addf5db371?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Chicken Alfredo",
                "Creamy fettuccine pasta with grilled chicken and parmesan cheese.",
                "Pasta",
                245,
                "https://images.unsplash.com/photo-1645112411341-6c4fd023714a?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Four Cheese Pizza",
                "Crispy pizza topped with mozzarella, parmesan, cheddar and blue cheese.",
                "Pizza",
                275,
                "https://images.unsplash.com/photo-1579751626657-72bc17010498?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Grilled Kofta",
                "Seasoned grilled kofta served with rice, salad and tahini sauce.",
                "Grill",
                240,
                "https://images.unsplash.com/photo-1544025162-d76694265947?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Chicken Shawarma",
                "Tender marinated chicken shawarma with garlic sauce and fresh vegetables.",
                "Middle Eastern",
                175,
                "https://images.unsplash.com/photo-1561651823-34feb02250e4?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Shrimp Pasta",
                "Italian pasta tossed with garlic shrimp, herbs and creamy sauce.",
                "Seafood",
                295,
                "https://images.unsplash.com/photo-1563379926898-05f4575a45d8?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Grilled Salmon",
                "Fresh salmon fillet grilled with lemon butter and seasonal vegetables.",
                "Seafood",
                345,
                "https://images.unsplash.com/photo-1467003909585-2f8a72700288?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Chicken Caesar Wrap",
                "Grilled chicken, lettuce, parmesan and Caesar dressing wrapped in fresh bread.",
                "Wraps",
                185,
                "https://images.unsplash.com/photo-1626700051175-6818013e1d4f?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "BBQ Beef Burger",
                "Double beef patty with smoked cheddar, crispy onions and BBQ sauce.",
                "Burgers",
                290,
                "https://images.unsplash.com/photo-1550547660-d9450f859349?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Truffle Mushroom Pasta",
                "Creamy mushroom pasta finished with parmesan and aromatic truffle oil.",
                "Pasta",
                320,
                "https://images.unsplash.com/photo-1473093295043-cdd812d0e601?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Pepperoni Pizza",
                "Wood-fired pizza with tomato sauce, mozzarella and spicy pepperoni.",
                "Pizza",
                250,
                "https://images.unsplash.com/photo-1628840042765-356cda07504e?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Greek Salad",
                "Fresh tomatoes, cucumber, olives, feta cheese and herbs.",
                "Salads",
                155,
                "https://images.unsplash.com/photo-1540420773420-3366772f4999?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Crispy Chicken",
                "Golden crispy chicken served with fries and homemade dipping sauce.",
                "Main Course",
                225,
                "https://images.unsplash.com/photo-1562967916-eb82221dfb92?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Grilled Mixed Platter",
                "A generous selection of grilled chicken, kofta and beef served with sides.",
                "Grill",
                385,
                "https://images.unsplash.com/photo-1544025162-d76694265947?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Lemon Herb Chicken",
                "Tender chicken breast marinated with lemon, garlic and fresh herbs.",
                "Main Course",
                250,
                "https://images.unsplash.com/photo-1598103442097-8b74394b95c6?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Spicy Chicken Pizza",
                "Mozzarella pizza topped with spicy chicken, peppers and fresh herbs.",
                "Pizza",
                265,
                "https://images.unsplash.com/photo-1593560708920-61dd98c8e1d8?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Mushroom Risotto",
                "Creamy Arborio rice cooked with mushrooms, parmesan and herbs.",
                "Risotto",
                285,
                "https://images.unsplash.com/photo-1476124369491-e7addf5db371?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Garlic Shrimp",
                "Sauteed shrimp with garlic, lemon, herbs and a light butter sauce.",
                "Seafood",
                315,
                "https://images.unsplash.com/photo-1565680018434-b513d5e5fd47?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Chicken Teriyaki",
                "Grilled chicken glazed with teriyaki sauce and served with steamed rice.",
                "Asian",
                255,
                "https://images.unsplash.com/photo-1547592180-85f173990554?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Beef Noodles",
                "Stir-fried noodles with tender beef, vegetables and Asian sauce.",
                "Asian",
                275,
                "https://images.unsplash.com/photo-1569718212165-3a8278d5f624?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Chicken Fried Rice",
                "Fried rice with grilled chicken, vegetables and aromatic Asian seasoning.",
                "Asian",
                215,
                "https://images.unsplash.com/photo-1603133872878-684f208fb84b?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Sushi Combo",
                "Chef selection of fresh sushi rolls with soy sauce and ginger.",
                "Sushi",
                390,
                "https://images.unsplash.com/photo-1579871494447-9811cf80d66c?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Salmon Sushi Roll",
                "Fresh salmon, avocado and sushi rice wrapped in nori.",
                "Sushi",
                320,
                "https://images.unsplash.com/photo-1579584425555-c3ce17fd4351?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Butter Chicken",
                "Tender chicken cooked in a rich tomato, butter and aromatic spice sauce.",
                "Indian",
                275,
                "https://images.unsplash.com/photo-1603894584373-5ac82b2ae398?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Chicken Biryani",
                "Fragrant basmati rice layered with spiced chicken and aromatic herbs.",
                "Indian",
                250,
                "https://images.unsplash.com/photo-1563379091339-03246963d51a?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Paneer Tikka",
                "Grilled Indian cheese marinated with yogurt, herbs and spices.",
                "Indian",
                220,
                "https://images.unsplash.com/photo-1567188040759-fb8a883dc6d8?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Falafel Plate",
                "Crispy falafel served with tahini, salad, pickles and fresh bread.",
                "Egyptian",
                120,
                "https://images.unsplash.com/photo-1593001874117-c99c0e2c5d1d?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Koshari",
                "Classic Egyptian koshari with rice, lentils, pasta, chickpeas and tomato sauce.",
                "Egyptian",
                110,
                "https://images.unsplash.com/photo-1596040033229-a9821ebd058d?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Molokhia with Chicken",
                "Traditional Egyptian molokhia served with tender chicken and fragrant rice.",
                "Egyptian",
                235,
                "https://images.unsplash.com/photo-1547592180-85f173990554?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Egyptian Fattah",
                "Layers of rice, toasted bread and tender meat with garlic vinegar sauce.",
                "Egyptian",
                275,
                "https://images.unsplash.com/photo-1544025162-d76694265947?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Grilled Sea Bass",
                "Fresh sea bass grilled with lemon, herbs and roasted vegetables.",
                "Seafood",
                360,
                "https://images.unsplash.com/photo-1519708227418-c8fd9a32b7a2?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Calamari Fritti",
                "Crispy fried calamari served with lemon and homemade dipping sauce.",
                "Seafood",
                295,
                "https://images.unsplash.com/photo-1604908176997-125f25cc6f3d?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Grilled Prawns",
                "Large prawns grilled with garlic, lemon and Mediterranean herbs.",
                "Seafood",
                380,
                "https://images.unsplash.com/photo-1559339352-11d035aa65de?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Mediterranean Bowl",
                "Fresh grains, grilled vegetables, feta, olives and herbs with house dressing.",
                "Healthy",
                205,
                "https://images.unsplash.com/photo-1512621776951-a57141f2eefd?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Avocado Chicken Bowl",
                "Grilled chicken, avocado, rice, greens and fresh vegetables.",
                "Healthy",
                235,
                "https://images.unsplash.com/photo-1547592180-85f173990554?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Halloumi Sandwich",
                "Grilled halloumi cheese with tomatoes, greens and herb sauce.",
                "Sandwiches",
                175,
                "https://images.unsplash.com/photo-1528735602780-2552fd46c7af?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Philly Beef Sandwich",
                "Sliced beef with caramelized onions, peppers and melted cheese.",
                "Sandwiches",
                245,
                "https://images.unsplash.com/photo-1553909489-cd47e0907980?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Club Sandwich",
                "Triple-layer sandwich with chicken, turkey, lettuce, tomato and cheese.",
                "Sandwiches",
                210,
                "https://images.unsplash.com/photo-1528735602780-2552fd46c7af?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "French Fries",
                "Crispy golden fries served with your choice of signature sauce.",
                "Sides",
                85,
                "https://images.unsplash.com/photo-1573080496219-bb080dd4f877?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Loaded Fries",
                "Crispy fries topped with cheese, beef pieces and signature sauce.",
                "Sides",
                145,
                "https://images.unsplash.com/photo-1585109649139-366815a0d713?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Garlic Bread",
                "Warm toasted bread with garlic butter, herbs and parmesan.",
                "Sides",
                95,
                "https://images.unsplash.com/photo-1619535860434-cf9b902a4a1e?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Chocolate Cake",
                "Rich chocolate cake with creamy chocolate frosting.",
                "Desserts",
                145,
                "https://images.unsplash.com/photo-1578985545062-69928b1d9587?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Tiramisu",
                "Classic Italian dessert with mascarpone, espresso and cocoa.",
                "Desserts",
                165,
                "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Cheesecake",
                "Creamy baked cheesecake served with fresh berries.",
                "Desserts",
                155,
                "https://images.unsplash.com/photo-1565958011703-44f9829ba187?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Pancakes",
                "Fluffy pancakes served with berries, honey and whipped cream.",
                "Breakfast",
                145,
                "https://images.unsplash.com/photo-1528207776546-365bb710ee93?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Eggs Benedict",
                "Poached eggs, toasted bread and creamy hollandaise sauce.",
                "Breakfast",
                185,
                "https://images.unsplash.com/photo-1608039829572-78524f79c4c7?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "French Toast",
                "Golden brioche French toast with berries and maple syrup.",
                "Breakfast",
                155,
                "https://images.unsplash.com/photo-1484723091739-30a097e8f929?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Acai Bowl",
                "Refreshing acai bowl topped with berries, banana and crunchy granola.",
                "Healthy",
                180,
                "https://images.unsplash.com/photo-1511690743698-d9d85f2fbf38?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Fresh Greek Yogurt",
                "Creamy Greek yogurt with honey, berries and toasted nuts.",
                "Breakfast",
                125,
                "https://images.unsplash.com/photo-1488477181946-6428a0291777?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Mango Smoothie",
                "Fresh mango blended with yogurt and a touch of honey.",
                "Drinks",
                105,
                "https://images.unsplash.com/photo-1505252585461-04db1eb84625?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Berry Smoothie",
                "Mixed berries blended with yogurt for a refreshing drink.",
                "Drinks",
                110,
                "https://images.unsplash.com/photo-1553530666-ba11a7da3888?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Iced Latte",
                "Smooth espresso with chilled milk served over ice.",
                "Drinks",
                95,
                "https://images.unsplash.com/photo-1461023058943-07fcbe16d735?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Cappuccino",
                "Rich espresso topped with steamed milk and silky foam.",
                "Drinks",
                85,
                "https://images.unsplash.com/photo-1572442388796-11668a67e53d?auto=format&fit=crop&w=900&q=85"
            ),

            new(
                "Fresh Lemon Mint",
                "Refreshing lemon juice blended with fresh mint and ice.",
                "Drinks",
                75,
                "https://images.unsplash.com/photo-1513558161293-cdaf765ed2fd?auto=format&fit=crop&w=900&q=85"
            )
        };
    }

    private record MenuTemplate(
        string Name,
        string Description,
        string Category,
        decimal Price,
        string ImageUrl);
}