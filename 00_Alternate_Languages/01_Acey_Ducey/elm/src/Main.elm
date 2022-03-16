module Main exposing (..)

import Browser
import Html exposing (..)
import Html.Attributes exposing (style, type_, value)
import Html.Events exposing (onInput)
import Random


main : Program () Model Msg
main =
    Browser.element
        { init = init
        , view = view
        , update = update
        , subscriptions = subscriptions
        }



-- Models


type alias Model =
    { money : Int
    , currentGame : Game
    , lastGame : Maybe Game
    , moneyBet : Int
    , error : Maybe String
    }


type alias Game =
    { cardA : Maybe Int
    , cardB : Maybe Int
    , cardC : Maybe Int
    }



-- Init


init : () -> ( Model, Cmd Msg )
init _ =
    ( { money = 100, currentGame = { cardA = Nothing, cardB = Nothing, cardC = Nothing }, lastGame = Nothing, moneyBet = 0, error = Nothing }, Random.generate NewCard newCard )



-- Messages


type Msg
    = BetMoney Int
    | UpdateBetValue String
    | NewCard Int
    | NewCardC Int
    | Play
    | NewGame



-- Update


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case msg of
        BetMoney bet ->
            ( { model | moneyBet = bet }, Cmd.none )

        UpdateBetValue value ->
            case String.toInt value of
                Just newValue ->
                    if newValue > model.money then
                        ( { model | error = Just "You cannot bet more than you have", moneyBet = model.money }, Cmd.none )

                    else
                        ( { model | moneyBet = newValue, error = Nothing }, Cmd.none )

                Nothing ->
                    ( { model | error = Just "Wrong input for bet" }, Cmd.none )

        NewCard card ->
            case model.currentGame.cardA of
                Nothing ->
                    let
                        currentGame =
                            model.currentGame
                    in
                    if card > 13 then
                        ( model, Random.generate NewCard newCard )

                    else
                        ( { model | currentGame = { currentGame | cardA = Just card } }, Random.generate NewCard newCard )

                Just cardA ->
                    let
                        currentGame =
                            model.currentGame
                    in
                    if card <= cardA then
                        ( { model | currentGame = { currentGame | cardA = Just card } }, Random.generate NewCard newCard )

                    else
                        ( { model | currentGame = { currentGame | cardB = Just card } }, Cmd.none )

        Play ->
            ( model, Random.generate NewCardC newCard )

        NewCardC card ->
            calculateNewState card model

        NewGame ->
            init ()


calculateNewState : Int -> Model -> ( Model, Cmd Msg )
calculateNewState cardC model =
    case model.currentGame.cardA of
        Just cardA ->
            case model.currentGame.cardB of
                Just cardB ->
                    let
                        currentGame =
                            model.currentGame
                    in
                    if cardC == cardA || cardC == cardB then
                        ( model, Random.generate NewCardC newCard )

                    else if cardA < cardC && cardC < cardB then
                        ( { model | money = model.money + model.moneyBet, currentGame = { currentGame | cardA = Nothing, cardB = Nothing }, lastGame = Just { cardA = model.currentGame.cardA, cardB = model.currentGame.cardB, cardC = Just cardC } }, Random.generate NewCard newCard )

                    else if model.moneyBet > model.money - model.moneyBet then
                        ( { model | money = model.money - model.moneyBet, moneyBet = model.money - model.moneyBet, currentGame = { currentGame | cardA = Nothing, cardB = Nothing }, lastGame = Just { cardA = model.currentGame.cardA, cardB = model.currentGame.cardB, cardC = Just cardC } }, Random.generate NewCard newCard )

                    else
                        ( { model | money = model.money - model.moneyBet, currentGame = { currentGame | cardA = Nothing, cardB = Nothing }, lastGame = Just { cardA = model.currentGame.cardA, cardB = model.currentGame.cardB, cardC = Just cardC } }, Random.generate NewCard newCard )

                Nothing ->
                    ( model, Cmd.none )

        Nothing ->
            ( model, Cmd.none )


subscriptions : Model -> Sub Msg
subscriptions _ =
    Sub.none



-- Views


view : Model -> Html Msg
view model =
    div centerHeadlineStyle
        [ showHeader
        , showGame model
        ]


showHeader : Html msg
showHeader =
    div headerStyle
        [ h1 [ style "font-size" "4rem" ] [ text "ACEY DUCEY CARD GAME" ]
        , div [] [ text "Creative Computing Morristown, New Jersey" ]
        , div []
            [ text """
        Acey-Ducey is played in the following manner. The Dealer (Computer) deals two cards face up. 
        You have an option to bet or not bet depending on whether or not you feel the card will have a value between the first two.
        If you do not want to bet, bet 0.
        """
            ]
        ]


showGame : Model -> Html Msg
showGame model =
    if model.money <= 0 then
        article gameStyle
            [ p cardContentPStyle [ text "You lose all you money" ]
            , button [ Html.Events.onClick NewGame, standardFontSize ] [ text "Again" ]
            ]

    else
        article gameStyle
            [ p cardContentPStyle [ text ("Currently you have " ++ String.fromInt model.money ++ " in your pocket.") ]
            , p cardContentPStyle [ text ("Card 1: " ++ cardToString model.currentGame.cardA) ]
            , p cardContentPStyle [ text ("Card 2: " ++ cardToString model.currentGame.cardB) ]
            , p cardContentPStyle [ text ("Your current bet is " ++ String.fromInt model.moneyBet) ]
            , input [ type_ "range", Html.Attributes.max (String.fromInt model.money), Html.Attributes.min "0", Html.Attributes.value (String.fromInt model.moneyBet), onInput UpdateBetValue ] []
            , button [ Html.Events.onClick Play, standardFontSize ] [ text "Play" ]
            , showLastGame model.lastGame
            , showError model.error
            ]


showLastGame : Maybe Game -> Html Msg
showLastGame game =
    case game of
        Nothing ->
            div [ standardFontSize ] [ text "This is your first game" ]

        Just value ->
            div []
                [ showLastWinLose value
                , p cardContentPStyle [ text ("Card 1: " ++ cardToString value.cardA) ]
                , p cardContentPStyle [ text ("Card 2: " ++ cardToString value.cardB) ]
                , p cardContentPStyle [ text ("Drawn Card: " ++ cardToString value.cardC) ]
                ]


showLastWinLose : Game -> Html Msg
showLastWinLose game =
    Maybe.map3 getGameStateMessage game.cardA game.cardB game.cardC |> Maybe.withDefault (text "something is wrong")


getGameStateMessage : Int -> Int -> Int -> Html Msg
getGameStateMessage cardA cardB cardC =
    if cardA < cardC && cardB > cardC then
        div [ standardFontSize ] [ text "You won :)" ]

    else
        div [ standardFontSize ] [ text "You loose :(" ]


showError : Maybe String -> Html Msg
showError value =
    case value of
        Just string ->
            p [ standardFontSize ] [ text string ]

        Nothing ->
            div [] []



-- Helper


cardToString : Maybe Int -> String
cardToString card =
    case card of
        Just value ->
            if value < 11 then
                String.fromInt value

            else
                case value of
                    11 ->
                        "Jack"

                    12 ->
                        "Queen"

                    13 ->
                        "King"

                    14 ->
                        "Ace"

                    _ ->
                        "impossible value"

        Nothing ->
            "-"


newCard : Random.Generator Int
newCard =
    Random.int 2 14



-- Styles


headerStyle : List (Attribute msg)
headerStyle =
    [ style "font-size" "2rem", style "text-align" "center" ]


cardContentPStyle : List (Attribute msg)
cardContentPStyle =
    [ style "font-size" "2rem"
    ]


gameStyle : List (Attribute msg)
gameStyle =
    [ style "width" "100%"
    , style "max-width" "70rem"
    ]


centerHeadlineStyle : List (Attribute msg)
centerHeadlineStyle =
    [ style "display" "grid"
    , style "place-items" "center"
    , style "margin" "2rem"
    ]


standardFontSize : Attribute msg
standardFontSize =
    style "font-size" "2rem"
