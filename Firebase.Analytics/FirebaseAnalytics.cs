using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Firebase.Analytics
{
    /// <summary>
    /// Analytics service interface.
    /// </summary>
    public sealed class FirebaseAnalytics
    {
        static Dictionary<string, FirebaseAnalytics> analyticsInstances;
        
        static FirebaseAnalytics defaultInstance;
        /// <summary>
        /// Default instance of the <see cref="FirebaseAnalytics"/> service for the default <see cref="FirebaseApp"/>.
        /// </summary>
        public static FirebaseAnalytics DefaultInstance
        {
            get
            {
                if (defaultInstance == null)
                    defaultInstance = GetInstance(FirebaseApp.DefaultInstance);
                return defaultInstance;
            }
        }
        /// <summary>
        /// Add Payment Info event.
        /// This event signifies that a user has submitted their payment information to your app.
        /// </summary>
        public static string EventAddPaymentInfo => "add_payment_info";
        /// <summary>
        /// E-Commerce Add To Cart event.
        /// This event signifies that an item was added to a cart for purchase.
        /// Add this event to a funnel with <see cref="EventEcommercePurchase "/> to gauge the effectiveness of your checParameter(out, If you supply the <see cref="ParameterValue"/> parameter), you must also supply the <see cref="ParameterCurrency"/> parameter so that revenue metrics can be computed accurately.
        /// </summary>
        public static string EventAddToCart => "add_to_cart";
        /// <summary>
        /// E-Commerce Add To Wishlist event.
        /// This event signifies that an item was added to a wishlist.
        /// Use this event to identify popular gift items in your app.Note: If you supply the <see cref="ParameterValue"/> parameter, you must also supply the <see cref="ParameterCurrency"/> parameter so that revenue metrics can be computed accurately.
        /// </summary>
        public static string EventAddToWishlist => "add_to_wishlist";
        /// <summary>
        /// App Open event.
        /// By logging this event when an App becomes active, developers can understand how often users leave and return during the course of a Session.Although Sessions are automatically reported, this event can provide further clarification around the continuous engagement of app-users.
        /// </summary>
        public static string EventAppOpen => "app_open";
        /// <summary>
        /// E-Commerce Begin Checkout event.
        /// This event signifies that a user has begun the process of checking out. Add this event to a funnel with your <see cref="EventEcommercePurchase"/> event to gauge the effectiveness of your checkout process.Note: If you supply the <see cref="ParameterValue"/> parameter, you must also supply the <see cref="ParameterCurrency"/> parameter so that revenue metrics can be computed accurately.
        /// </summary>
        public static string EventBeginCheckout => "begin_checkout";
        /// <summary>
        /// Campaign Detail event.
        /// Log this event to supply the referral details of a re-engagement campaign.
        /// Note: you must supply at least one of the required parameters <see cref="ParameterSource"/>, <see cref="ParameterMedium"/> or <see cref="ParameterCampaign"/>.
        /// </summary>
        public static string EventCampaignDetails => "campaign_details";
        /// <summary>
        /// A user completes a checkout step
        /// </summary>
        public static string EventCheckoutProgress => "checkout_progress";
        /// <summary>
        /// Earn Virtual Currency event.
        /// This event tracks the awarding of virtual currency in your app.
        /// Log this along with <see cref="EventSpendVirtualCurrency"/> to better understand your economy.
        /// </summary>
        public static string EventEarnVirtualCurrency => "earn_virtual_currency";
        /// <summary>
        /// E-Commerce Purchase event.
        /// This event signifies that an item was purchased by a user.
        /// Note: This is different from the in-app purchase event, which is reported automatically for App Store-based apps.
        /// Note: If you supply the <see cref="ParameterValue"/> parameter, you must also supply the <see cref="ParameterCurrency"/> parameter so that revenue metrics can be computed accurately.
        /// </summary>
        public static string EventEcommercePurchase => "purchase";
        /// <summary>
        /// Generate Lead event.
        /// Log this event when a lead has been generated in the app to understand the efficacy of your install and re-engagement campaigns.
        /// Note: If you supply the <see cref="ParameterValue"/> parameter, you must also supply the <see cref="ParameterCurrency"/> parameter so that revenue metrics can be computed accurately.
        /// </summary>
        public static string EventGenerateLead => "generate_lead";
        /// <summary>
        /// Join Group event.
        /// Log this event when a user joins a group such as a guild, team or family.
        /// Use this event to analyze how popular certain groups or social features are in your app.
        /// </summary>
        public static string EventJoinGroup => "join_group";
        /// <summary>
        /// Level End event.
        /// Log this event when the user finishes a level.
        /// </summary>
        public static string EventLevelEnd => "level_end";
        /// <summary>
        /// Level Start event.
        /// Log this event when the user starts a new level.
        /// </summary>
        public static string EventLevelStart => "level_start";
        /// <summary>
        /// Level Up event.
        /// This event signifies that a player has leveled up in your gaming app.
        /// It can help you gauge the level distribution of your userbase and help you identify certain levels that are difficult to pass.
        /// </summary>
        public static string EventLevelUp => "level_up";
        /// <summary>
        /// Login event.
        /// Apps with a login feature can report this event to signify that a user has logged in.
        /// </summary>
        public static string EventLogin => "login";
        /// <summary>
        /// Post Score event.
        /// Log this event when the user posts a score in your gaming app.
        /// This event can help you understand how users are actually performing in your game and it can help you correlate high scores with certain audiences or behaviors.
        /// </summary>
        public static string EventPostScore => "post_score";
        /// <summary>
        /// Present Offer event.
        /// This event signifies that the app has presented a purchase offer to a user.
        /// Add this event to a funnel with the <see cref="EventAddToCart"/> and <see cref="EventEcommercePurchase"/> to gauge your conversion process.
        /// Note: If you supply the <see cref="ParameterValue"/> parameter, you must also supply the <see cref="ParameterCurrency"/> parameter so that revenue metrics can be computed accurately.
        /// </summary>
        public static string EventPresentOffer => "present_offer";
        /// <summary>
        /// E-Commerce Purchase Refund event.
        /// This event signifies that an item purchase was refunded.
        /// Note: If you supply the <see cref="ParameterValue"/> parameter, you must also supply the <see cref="ParameterCurrency"/> parameter so that revenue metrics can be computed accurately.
        /// </summary>
        public static string EventPurchaseRefund => "refund";
        /// <summary>
        /// Remove from cart event, a user removed an item from cart.
        /// </summary>
        public static string EventRemoveFromCart => "remove_from_cart";
        /// <summary>
        /// Search event.
        /// Apps that support search features can use this event to contextualize search operations by supplying the appropriate, corresponding parameters.
        /// This event can help you identify the most popular content in your app.
        /// </summary>
        public static string EventSearch => "search";
        /// <summary>
        /// Select Content event.
        /// This general purpose event signifies that a user has selected some content of a certain type in an app.
        /// The content can be any object in your app. 
        /// This event can help you identify popular content and categories of content in your app.
        /// </summary>
        public static string EventSelectContent => "select_content";
        /// <summary>
        /// An event when a user has selected an option value for a given checkout step
        /// </summary>
        public static string EventSetCheckoutOption => "set_checkout_option";
        /// <summary>
        /// Share event.
        /// Apps with social features can log the Share event to identify the most viral content.
        /// </summary>
        public static string EventShare => "share";
        /// <summary>
        /// Sign Up event.
        /// This event indicates that a user has signed up for an account in your app.
        /// The parameter signifies the method by which the user signed up. 
        /// Use this event to understand the different behaviors between logged in and logged out users.
        /// </summary>
        public static string EventSignUp => "sign_up";
        /// <summary>
        /// Spend Virtual Currency event.
        /// This event tracks the sale of virtual goods in your app and can help you identify which goods are the most popular objects of purchase.
        /// </summary>
        public static string EventSpendVirtualCurrency => "spend_virtual_currency";
        /// <summary>
        /// Tutorial Begin event.
        /// This event signifies the start of the on-boarding process in your app.
        /// Use this in a funnel with <see cref="EventTutorialComplete"/> to understand how many users complete this process and move on to the full app experience.
        /// </summary>
        public static string EventTutorialBegin => "tutorial_begin";
        /// <summary>
        /// Tutorial End event.
        /// Use this event to signify the user's completion of your app's on-boarding process.
        /// Add this to a funnel with <see cref="EventTutorialBegin"/> to gauge the completion rate of your on-boarding process.
        /// </summary>
        public static string EventTutorialComplete => "tutorial_complete";
        /// <summary>
        /// Unlock Achievement event.
        /// Log this event when the user has unlocked an achievement in your game.
        /// Since achievements generally represent the breadth of a gaming experience, this event can help you understand how many users are experiencing all that your game has to offer.
        /// </summary>
        public static string EventUnlockAchievement => "unlock_achievement";
        /// <summary>
        /// View Item event.
        /// This event signifies that some content was shown to the user.
        /// This content may be a product, a webpage or just a simple image or text.
        /// Use the appropriate parameters to contextualize the event. 
        /// Use this event to discover the most popular items viewed in your app.
        /// Note: If you supply the <see cref="ParameterValue"/> parameter, you must also supply the <see cref="ParameterCurrency"/> parameter so that revenue metrics can be computed accurately.
        /// </summary>
        public static string EventViewItem => "view_item";
        /// <summary>
        /// View Item List event.
        /// Log this event when the user has been presented with a list of items of a certain category.
        /// </summary>
        public static string EventViewItemList => "view_item_list";
        /// <summary>
        /// View Search Results event.
        /// Log this event when the user has been presented with the results of a search.
        /// </summary>
        public static string EventViewSearchResults => "view_search_results";
        /// <summary>
        /// A user clicks on an internal promotion
        /// </summary>
        public static string EventViewPromotion => "view_promotion";
        /// <summary>
        /// A timed activity completes
        /// </summary>
        public static string EventTimingComplete => "timing_complete";
        /// <summary>
        /// A user loads a new screen or new content.
        /// </summary>
        public static string EventScreenView => "screen_view";
        /// <summary>
        /// A user loads a web page.
        /// </summary>
        public static string EventPageView => "page_view";
        /// <summary>
        /// An error occured, this event is helpful to track the errors/exceptions the user encounter in the app, and opt to fix them early before being reported by users.
        /// </summary>
        public static string EventExceptionOrError => "exception";
        /// <summary>
        /// Achievement ID (string).
        /// </summary>
        public static string ParameterAchievementId => "achievement_id";
        /// <summary>
        /// Ad Network Click ID (string).
        /// Used for network-specific click IDs which vary in format.
        /// </summary>
        public static string ParameterAdNetworkClickID => "ad_network_click_id";
        /// <summary>
        /// The store or affiliation from which this transaction occurred (string).
        /// </summary>
        public static string ParameterAffiliation => "affiliation";
        /// <summary>
        /// Campaign custom parameter (string).
        /// Used as a method of capturing custom data in a campaign.
        /// Use varies by network.
        /// </summary>
        public static string ParameterCP1 => "cp1";
        /// <summary>
        /// The individual campaign name, slogan, promo code, etc.
        /// Some networks have pre-defined macro to capture campaign information, otherwise can be populated by developer.
        /// Highly Recommended(string).
        /// </summary>
        public static string ParameterCampaign => "campaign";
        /// <summary>
        /// Character used in game (string).
        /// </summary>
        public static string ParameterCharacter => "character";
        /// <summary>
        /// Some option on a step in an ecommerce flow (string).
        /// </summary>
        public static string ParameterCheckoutOption => "checkout_option";
        /// <summary>
        /// The checkout step (1..N) (unsigned 64-bit integer)(ulong)
        /// </summary>
        public static string ParameterCheckoutStep => "checkout_step";
        /// <summary>
        /// Campaign content (string).
        /// </summary>
        public static string ParameterContent => "content";
        /// <summary>
        /// Type of content selected (string).
        /// </summary>
        public static string ParameterContentType => "content_type";
        /// <summary>
        /// Coupon code for a purchasable item (string).
        /// </summary>
        public static string ParameterCoupon => "coupon";
        /// <summary>
        /// The name of a creative used in a promotional spot (string).
        /// </summary>
        public static string ParameterCreativeName => "creative_name";
        /// <summary>
        /// The name of a creative slot (string).
        /// </summary>
        public static string ParameterCreativeSlot => "creative_slot";
        /// <summary>
        /// Purchase currency in 3-letter ISO_4217 format
        /// </summary>
        public static string ParameterCurrency => "currency";
        /// <summary>
        /// Flight or Travel destination (string).
        /// </summary>
        public static string ParameterDestination => "destination";
        /// <summary>
        /// The arrival date, check-out date or rental end date for the item.
        /// This should be in YYYY-MM-DD format(string).
        /// </summary>
        public static string ParameterEndDate => "end_date";
        /// <summary>
        /// Indicates that the associated event should either extend the current session or start a new session if no session was active when the event was logged.
        /// Specify YES to extend the current session or to start a new session; any other value will not extend or start a session.
        /// </summary>
        public static string ParameterExtendSession => "extend_session";
        /// <summary>
        /// Flight number for travel events (string).
        /// </summary>
        public static string ParameterFlightNumber => "flight_number";
        /// <summary>
        /// Group/guild/team id...
        /// </summary>
        public static string ParameterGroupId => "group_id";
        /// <summary>
        /// Index of an item in a list (signed 64-bit integer)(long).
        /// </summary>
        public static string ParameterIndex => "index";
        /// <summary>
        /// Item brand (string).
        /// </summary>
        public static string ParameterItemBrand => "brand";
        /// <summary>
        /// Item category (string).
        /// </summary>
        public static string ParameterItemCategory => "category";
        /// <summary>
        /// Unique ID/SKU for the item
        /// </summary>
        public static string ParameterItemId => "id";
        /// <summary>
        /// The list in which the item was presented to the user (string).
        /// </summary>
        public static string ParameterItemList => "item_list";
        /// <summary>
        /// Location of the item
        /// </summary>
        public static string ParameterItemLocationId => "location_id";
        /// <summary>
        /// Item name (string).
        /// </summary>
        public static string ParameterItemName => "name";
        /// <summary>
        /// Item variant (string).
        /// </summary>
        public static string ParameterItemVariant => "item_variant";
        /// <summary>
        /// Level in game (signed 64-bit integer)(long)
        /// </summary>
        public static string ParameterLevel => "level";
        /// <summary>
        /// The name of a level in a game (string).
        /// </summary>
        public static string ParameterLevelName => "level_name";
        /// <summary>
        /// Location (string).
        /// The Google Place ID that corresponds to the associated event.
        /// Alternatively, you can supply your own custom Location ID.
        /// </summary>
        public static string ParameterLocation => "location";
        /// <summary>
        /// The advertising or marParameter(eting, cpc, banner, email), push.
        /// Highly recommended(string).
        /// </summary>
        public static string ParameterMedium => "medium";
        /// <summary>
        /// A particular approach used in an operation; for example, "facebook" or "email" in the context of a sign_up or login event.(string).
        /// </summary>
        public static string ParameterMethod => "method";
        /// <summary>
        /// Number of nights staying at hotel (signed 64-bit integer)(long).
        /// </summary>
        public static string ParameterNumberOfNights => "number_of_nights";
        /// <summary>
        /// Number of passengers traveling (signed 64-bit integer)(long).
        /// </summary>
        public static string ParameterNumberOfPassengers => "number_of_passengers";
        /// <summary>
        /// Number of rooms for travel events (signed 64-bit integer)(long).
        /// </summary>
        public static string ParameterNumberOfRooms => "number_of_rooms";
        /// <summary>
        /// Flight or Travel origin (string).
        /// </summary>
        public static string ParameterOrigin => "origin";
        /// <summary>
        /// Purchase price (double).
        /// </summary>
        public static string ParameterPrice => "price";
        /// <summary>
        /// Purchase quantity (signed 64-bit integer)(long).
        /// </summary>
        public static string ParameterQuantity => "quantity";
        /// <summary>
        /// Score in game (signed 64-bit integer)(long).
        /// </summary>
        public static string ParameterScore => "score";
        /// <summary>
        /// The search string/keywords used (string).
        /// </summary>
        public static string ParameterSearchTerm => "search_term";

        /// <summary>
        /// Shipping cost (double).
        /// </summary>
        public static string ParameterShipping => "shipping";
        /// <summary>
        /// Sign up method (string).
        /// </summary>
        public static string ParameterSignUpMethod => "sign_up_method";
        /// <summary>
        /// The origin of your traffic, such as an Ad network (for example, google) or partner (urban airship).
        /// Identify the advertiser, site, publication, etc.that is sending traffic to your property.Highly recommended(string).
        /// </summary>
        public static string ParameterSource => "source";

        /// <summary>
        /// The departure date, check-in date or rental start date for the item.
        /// This should be in YYYY-MM-DD format(string).
        /// </summary>
        public static string ParameterStartDate => "start_date";
        /// <summary>
        /// The result of an operation.
        /// Specify 1 to indicate success and 0 to indicate failure(unsigned integer).
        /// </summary>
        public static string ParameterSuccess => "success";
        /// <summary>
        /// Tax amount for transaction
        /// </summary>
        public static string ParameterTax => "tax";

        /// <summary>
        /// If you're manually tagging keyword campaigns, you should use utm_term to specify the keyword (string).
        /// </summary>
        public static string ParameterTerm => "term";
        /// <summary>
        /// Transaction ID
        /// </summary>
        public static string ParameterTransactionId => "transaction_id";
        /// <summary>
        /// Travel class (string).
        /// </summary>
        public static string ParameterTravelClass => "travel_class";
        /// <summary>
        /// A context-specific numeric value which is accumulated automatically for each event type.
        /// This is a general purpose parameter that is useful for accumulating a key metric that pertains to an event. 
        /// Examples include revenue, distance, time and points. 
        /// Value should be specified as signed 64-bit integer or double. 
        /// Notes: Values for pre-defined currency-related events(such as <see cref="EventAddToCart"/>) should be supplied using double and must be accompanied by a <see cref="ParameterCurrency"/> parameter.
        /// The valid range of accumulated values is [-9,223,372,036,854.77, 9,223,372,036,854.77]. 
        /// Supplying a non-numeric value, omitting the corresponding <see cref="ParameterCurrency"/> parameter, or supplying an invalid currency code for conversion events will cause that conversion to be omitted from reporting.
        /// </summary>
        public static string ParameterValue => "value";

        /// <summary>
        /// Name of in-app/in-game currency type (string).
        /// </summary>
        public static string ParameterVirtualCurrencyName => "virtual_currency_name";
        /// <summary>
        /// Required <see cref="EventScreenView"/> event parameter (along with "app_name").
        /// </summary>
        public static string ParameterScreenName => "screen_name";

        /// <summary>
        /// Used for measuring exceptions, 1 = is fatal ,0 = is not fatal.
        /// </summary>
        public static string ParameterFatal => "fatal";
       /// <summary>
       /// Label Event parameter.
       /// </summary>
        public static string ParameterEventLabel => "event_label";
        /// <summary>
        /// Category Event parameter.
        /// </summary>
        public static string ParameterEventCategory => "event_category";
        /// <summary>
        /// Description Event parameter.
        /// </summary>
        public static string ParameterDescription => "description";
        /// <summary>
        /// Content Id (context specific)
        /// </summary>
        public static string ParameterContentID => "content_id";


        static FirebaseAnalytics()
        {
            analyticsInstances = new Dictionary<string, FirebaseAnalytics>();
        }
        /// <summary>
        /// The <see cref="FirebaseApp"/> associated with the <see cref="FirebaseAnalytics"/> service instance.
        /// </summary>
        public FirebaseApp App { get; }
        private FirebaseAnalytics(FirebaseApp app)
        {
            App = app;
            // Lets enable analytics collections here because by default on web it is disabled, whereas on unity by default it is enabled.
            SetAnalyticsCollectionEnabled(true);
        }

        /// <summary>
        /// Log an event with associated parameters.
        /// An Event is an important occurrence in your app that you want to measure.
        /// You can report up to 500 different types of events per app and you can associate up to 25 unique parameters with each Event type.
        /// Some common events are in the reference guide via the FirebaseAnalytics.Event* constants, but you may also choose to specify custom event types that are associated with your specific app.
        /// </summary>
        /// <param name="name">Name of the event to log. Should contain 1 to 32 alphanumeric characters or underscores. The name must start with an alphabetic character. Some event names are reserved. See Analytics Events for the list of reserved event names. The "firebase_" prefix is reserved and should not be used. Note that event names are case-sensitive and that logging two events whose names differ only in case will result in two distinct events.</param>
        /// <param name="options">Optinal analytics call settings to be used when logging the event [This is effective only in WebGL Build].</param>
        /// <param name="parameters">A parameter array of <see cref="Parameter"/> instances.</param>
        public void LogEvent(string name, AnalyticsCallOptions options = null, params Parameter[] parameters)
        {
            Dictionary<string, object> eventParams = new Dictionary<string, object>(parameters == null ? 0 : parameters.Length);
            if(parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (!eventParams.ContainsKey(parameters[i].Name))
                        eventParams.Add(parameters[i].Name, parameters[i].Value);
                }
            }
            string eventParamsJson = JsonConvert.SerializeObject(eventParams);
            string optionsJson = options == null ? null : JsonConvert.SerializeObject(options);
            AnalyticsPInvoke.LogAnalyticsEvent_WebGL(App.Name, name, eventParamsJson, optionsJson);
        }

        /// <summary>
        /// Use gtag 'config' command to set 'screen_name'.
        /// </summary>
        /// <param name="screenName"></param>
        /// <param name="options">Optional options about logging this screen name.</param>
        public void SetCurrentScreen(string screenName, AnalyticsCallOptions options = null)
        {
            AnalyticsPInvoke.SetAnalyticsCurrentScreen_WebGL(App.Name, screenName, options == null ? null : JsonConvert.SerializeObject(options));
        }
        /// <summary>
        /// Sets the user ID property.
        /// This feature must be used in accordance with Google's Privacy Policy
        /// </summary>
        /// <param name="userId">
        /// The user ID associated with the user of this app on this device.
        /// The user ID must be non-empty and no more than 256 characters long.
        /// Setting userId to NULL or nullptr removes the user ID.
        /// </param>
        public void SetUserIdInstance(string userId)
        {
            AnalyticsPInvoke.SetAnalyticsUserId_WebGL(App.Name, userId);
        }
        /// <summary>
        /// Sets whether analytics collection is enabled for this app on this device.
        /// This setting is persisted across app sessions.By default it is enabled.
        /// </summary>
        /// <param name="enabled">true to enable analytics collection, false to disable.</param>
        public void SetAnalyticsCollectionEnabledInstance(bool enabled)
        {
            AnalyticsPInvoke.SetAnalyticsCollectionEnabled_WebGL(App.Name, enabled);
        }
        /// <summary>
        /// Set a user property to the given value.
        /// Properties associated with a user allow a developer to segment users into groups that are useful to their application.
        /// Up to 25 properties can be associated with a user.
        /// Suggested property names are listed Analytics User Properties (user_property_names.h) but you're not limited to this set. 
        /// For example, the "gamertype" property could be used to store the type of player where a range of values could be "casual", "mid_core", or "core".
        /// </summary>
        /// <param name="name">Name of the user property to set. This must be a combination of letters and digits (matching the regular expression [a-zA-Z0-9] between 1 and 40 characters long starting with a letter [a-zA-Z] character.</param>
        /// <param name="property">Value to set the user property to. Set this argument to NULL or nullptr to remove the user property. The value can be between 1 to 100 characters long.</param>
        public void SetUserPropertyInstance(string name, string property)
        {
            AnalyticsPInvoke.SetAnalyticsUserProperty_WebGL(App.Name, name, property);
        }
        #region Static Methods
        /// <summary>
        /// Clears all analytics data for this app from the device and resets the app instance id.
        /// </summary>
        public static void ResetAnalyticsData()
        {
            PlatformHandler.NotifyWebGLFeatureDoesntHaveAMatch();
        }
        /// <summary>
        /// Log an event with one string parameter.
        /// </summary>
        /// <param name="name">Name of the event to log. Should contain 1 to 40 alphanumeric characters or underscores. The name must start with an alphabetic character. Some event names are reserved. See the FirebaseAnalytics.Event properties for the list of reserved event names. The "firebase_" prefix is reserved and should not be used. Note that event names are case-sensitive and that logging two events whose names differ only in case will result in two distinct events.</param>
        /// <param name="parameterName">Name of the parameter to log. For more information, see <see cref="Parameter"/>.</param>
        /// <param name="parameterValue">Value of the parameter to log.</param>
        public static void LogEvent(string name, string parameterName, string parameterValue)
        {
            LogEvent(name, new Parameter(parameterName, parameterValue));
        }
        /// <summary>
        /// Log an event with one float parameter.
        /// </summary>
        /// <param name="name">Name of the event to log. Should contain 1 to 40 alphanumeric characters or underscores. The name must start with an alphabetic character. Some event names are reserved. See the FirebaseAnalytics.Event properties for the list of reserved event names. The "firebase_" prefix is reserved and should not be used. Note that event names are case-sensitive and that logging two events whose names differ only in case will result in two distinct events.</param>
        /// <param name="parameterName">Name of the parameter to log. For more information, see <see cref="Parameter"/>.</param>
        /// <param name="parameterValue">Value of the parameter to log.</param>
        public static void LogEvent(string name, string parameterName, double parameterValue)
        {
            LogEvent(name, new Parameter(parameterName, parameterValue));
        }

        /// <summary>
        /// Log an event with one 64-bit integer parameter.
        /// </summary>
        /// <param name="name">Name of the event to log. Should contain 1 to 40 alphanumeric characters or underscores. The name must start with an alphabetic character. Some event names are reserved. See the FirebaseAnalytics.Event properties for the list of reserved event names. The "firebase_" prefix is reserved and should not be used. Note that event names are case-sensitive and that logging two events whose names differ only in case will result in two distinct events.</param>
        /// <param name="parameterName">Name of the parameter to log. For more information, see <see cref="Parameter"/>.</param>
        /// <param name="parameterValue">Value of the parameter to log.</param>
        public static void LogEvent(string name, string parameterName, long parameterValue)
        {
            LogEvent(name, new Parameter(parameterName, parameterValue));
        }

        /// <summary>
        /// Log an event with one integer parameter (stored as a 64-bit integer).
        /// </summary>
        /// <param name="name">Name of the event to log. Should contain 1 to 40 alphanumeric characters or underscores. The name must start with an alphabetic character. Some event names are reserved. See the FirebaseAnalytics.Event properties for the list of reserved event names. The "firebase_" prefix is reserved and should not be used. Note that event names are case-sensitive and that logging two events whose names differ only in case will result in two distinct events.</param>
        /// <param name="parameterName">Name of the parameter to log. For more information, see <see cref="Parameter"/>.</param>
        /// <param name="parameterValue">Value of the parameter to log.</param>
        public static void LogEvent(string name, string parameterName, int parameterValue)
        {
            LogEvent(name, new Parameter(parameterName, parameterValue));
        }
        /// <summary>
        /// Log an event with no parameters.
        /// </summary>
        /// <param name="name">Name of the event to log. Should contain 1 to 40 alphanumeric characters or underscores. The name must start with an alphabetic character. Some event names are reserved. See the FirebaseAnalytics.Event properties for the list of reserved event names. The "firebase_" prefix is reserved and should not be used. Note that event names are case-sensitive and that logging two events whose names differ only in case will result in two distinct events.</param>
        public static void LogEvent(string name)
        {
            LogEvent(name, new Parameter[0]);
        }
        /// <summary>
        /// Log an event with an array (could be empty) of parameters.
        /// </summary>
        /// <param name="name">Name of the event to log. Should contain 1 to 40 alphanumeric characters or underscores. The name must start with an alphabetic character. Some event names are reserved. See the FirebaseAnalytics.Event properties for the list of reserved event names. The "firebase_" prefix is reserved and should not be used. Note that event names are case-sensitive and that logging two events whose names differ only in case will result in two distinct events.</param>
        /// <param name="parameters">Event parameters.</param>
        public static void LogEvent(string name, params Parameter[] parameters)
        {
            DefaultInstance.LogEvent(name, AnalyticsCallOptions.Default, parameters);
        }
        /// <summary>
        /// Set a user property to the given value.
        /// Properties associated with a user allow a developer to segment users into groups that are useful to their application.
        /// Up to 25 properties can be associated with a user.
        /// Suggested property names are listed Analytics User Properties (user_property_names.h) but you're not limited to this set. 
        /// For example, the "gamertype" property could be used to store the type of player where a range of values could be "casual", "mid_core", or "core".
        /// </summary>
        /// <param name="name">Name of the user property to set. This must be a combination of letters and digits (matching the regular expression [a-zA-Z0-9] between 1 and 40 characters long starting with a letter [a-zA-Z] character.</param>
        /// <param name="property">Value to set the user property to. Set this argument to NULL or nullptr to remove the user property. The value can be between 1 to 100 characters long.</param>
        public static void SetUserProperty(string name, string property)
        {
            DefaultInstance.SetUserPropertyInstance(name, property);
        }
        /// <summary>
        /// Sets whether analytics collection is enabled for this app on this device.
        /// This setting is persisted across app sessions.By default it is enabled.
        /// </summary>
        /// <param name="enabled">true to enable analytics collection, false to disable.</param>
        public static void SetAnalyticsCollectionEnabled(bool enabled)
        {
            DefaultInstance.SetAnalyticsCollectionEnabledInstance(enabled);
        }
        /// <summary>
        /// Sets the user ID property.
        /// This feature must be used in accordance with Google's Privacy Policy
        /// </summary>
        /// <param name="userId">
        /// The user ID associated with the user of this app on this device.
        /// The user ID must be non-empty and no more than 256 characters long.
        /// Setting userId to NULL or nullptr removes the user ID.
        /// </param>
        public static void SetUserId(string userId)
        {
            DefaultInstance.SetUserIdInstance(userId);
        }
        /// <summary>
        /// Use gtag 'config' command to set 'screen_name'.
        /// </summary>
        /// <param name="screenName">The name of the current screen. Set to nullptr to clear the current screen name. Limited to 100 characters.</param>
        /// <param name="screenClass">The name of the screen class. If you specify nullptr for this, it will use the default. On Android, the default is the class name of the current Activity. On iOS, the default is the class name of the current UIViewController. Limited to 100 characters.</param>
        public static void SetCurrentScreen(string screenName, string screenClass)
        {
            DefaultInstance.SetCurrentScreen(screenName, AnalyticsCallOptions.Default);
        }
        /// <summary>
        /// Sets the duration of inactivity that terminates the current session.
        /// The default value is 30 minutes.
        /// </summary>
        /// <param name="timeSpan">The duration of inactivity that terminates the current session.</param>
        public static void SetSessionTimeoutDuration(TimeSpan timeSpan)
        {
            PlatformHandler.NotifyFeatureIsUselessInWebGL();
        }
        /// <summary>
        /// Get an existing or create a new <see cref="FirebaseAnalytics"/> instance for the specified app instance,
        /// if the app instance is null the default <see cref="FirebaseApp"/> is used and the default <see cref="FirebaseAnalytics"/> is returned.
        /// </summary>
        /// <param name="app">The app for which to create <see cref="FirebaseAnalytics"/> instance.</param>
        /// <returns>An existing or newly created <see cref="FirebaseAnalytics"/> service for the specified app.</returns>
        public static FirebaseAnalytics GetInstance(FirebaseApp app)
        {
            app = app ?? FirebaseApp.DefaultInstance;
            if (analyticsInstances.TryGetValue(app.Name, out FirebaseAnalytics analytics))
                return analytics;
            analytics = new FirebaseAnalytics(app);
            analyticsInstances[app.Name] = analytics;
            return analytics;
        }

        /// <summary>
        /// Configures Firebase Analytics to use custom gtag or dataLayer names. Intended to be used if gtag.js script has been installed on this page independently of Firebase Analytics, and is using non-default names for either the gtag function or for dataLayer. 
        /// Must be called before calling any <see cref="FirebaseAnalytics"/> method/property or field or it won't have any effect.
        /// </summary>
        /// <param name="settings"></param>
        public static void SetSettings(SettingsOptions settings)
        {
            AnalyticsPInvoke.SetAnalyticsSettings_WebGL(JsonConvert.SerializeObject(settings));
        }

        /// <summary>
        /// Get the instance ID from the analytics service.
        /// This is not the same ID as the ID returned by Firebase.InstanceId.FirebaseInstanceId.
        /// Object which can be used to retrieve the analytics instance ID.
        /// </summary>
        /// <returns></returns>
        public static Task<string> GetAnalyticsInstanceIdAsync()
        {
            PlatformHandler.NotifyWebGLFeatureDoesntHaveAMatch();
            TaskCompletionSource<string> task = new TaskCompletionSource<string>();
            task.SetException(new System.NotImplementedException());
            return task.Task;
        }
        #endregion
    }
}
