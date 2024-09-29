## WbCardParser

WbCardParser is a project designed to scrape product cards from the Wildberries website using ASP.NET and Selenium. The project consists of two main components:

    Parser — an ASP.NET application that uses Selenium to open product cards in the browser and extract data.
    Bot — receives product cards, sends them to the parser via HTTP, and handles moderation before publishing to a Telegram channel.

## Features
### Parser
The parser opens product pages on Wildberries through Selenium and extracts the following data:

    Article — product article number.
    Name — product name.
    Description — product description.
    Rate — product rating.
    RatesCount — number of ratings.
    Brand — product brand.
    Price — current price.
    OldPrice — previous price.
    CreatedBy — ID of the user who created the card.
    Status — card status.
    Images — collection of product images.

The extracted data is then stored in the database.
### Bot

    The bot receives product card data.
    Sends the data to the parser for processing via HTTP.
    Once the product card is processed, the bot allows:
        Approve — the product is published in a Telegram channel.
        Reject — the product is not published.

## Installation and Setup
### Dependencies

To run the project, you'll need:

    Docker
    Docker Compose

## Configuration

Before running the project, create a `.env` file based on the `.env.sample` file. This file contains configuration settings needed for deployment via Docker Compose.

Running with Docker Compose

To start the project using Docker Compose, run the following command in the root of the project:

``` bash
docker-compose up --build
```
This will build and run all necessary containers (bot and parser).
## Interaction
Bot Workflow

    Send the product card to the bot.
    The bot forwards the data to the parser, which processes the card and returns the result.
    After receiving the processed card, the bot will prompt you to:
        Approve the card — the product is published to a Telegram channel.
        Reject the card — it will not be published.

Project Structure

    Parser — ASP.NET application using Selenium to scrape product data from Wildberries.
    Bot — manages product cards, interacts with the parser, and handles card moderation before publication.

Notes

    The project uses Selenium, but the browser and driver setup are automated through Docker.
    All data is stored in a database, and the model structure can be adapted to your needs.
