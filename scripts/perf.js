import { randomIntBetween } from "https://jslib.k6.io/k6-utils/1.2.0/index.js";
import { check } from "k6";
import http from "k6/http";

export const options = {
  vus: 100,
  duration: "1m",
};

export default function () {
  // Generate a random counter ID between 1 and 1000
  const counterId = randomIntBetween(1, 1000);
  const baseUrl = "http://localhost:5025/counter";

  // POST to increment the counter
  const postUrl = `${baseUrl}/${counterId}`;
  const postResponse = http.post(postUrl);

  check(postResponse, {
    "POST status is 200": (r) => r.status === 200,
    "POST response is a number": (r) => !isNaN(Number(r.body)),
  });

  // Small delay to simulate user think time
  //  sleep(0.1);

  // GET to retrieve the counter value
  const getUrl = `${baseUrl}/${counterId}`;
  const getResponse = http.get(getUrl);

  check(getResponse, {
    "GET status is 200": (r) => r.status === 200,
    "GET response is a number": (r) => !isNaN(Number(r.body)),
  });

  // Add a small random delay between iterations
  //sleep(randomIntBetween(1, 3) / 10);
}
