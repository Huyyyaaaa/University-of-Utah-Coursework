<?php

namespace App\Http\Controllers\MobileAuth;

use App\Http\Controllers\Controller;
use GuzzleHttp\Client;

class MobileLoginController extends Controller
{
    public function login()
    {
        $attributes = $this->validateRequest();

        $client = new Client([
            'base_uri' => config('app.url'),
        ]);

        $response = $client->post('/oauth/token', [
            'headers' => [
                'Content-Type' => 'application/json',
                'Accept'       => 'application/json',
            ],
            'json'    => [
                'client_id'     => config('app.mobile.client_id'),
                'client_secret' => config('app.mobile.client_secret'),
                'grant_type'    => 'password',
                'username'      => $attributes['username'],
                'password'      => $attributes['password']
            ],
        ]);

        $result = json_decode($response->getBody()->getContents());

        return $this->success('Successfully retrieved access and refresh tokens!', [
            'access_token'  => $result->access_token,
            'refresh_token' => $result->refresh_token,
        ]);
    }

    private function validateRequest()
    {
        $attributes = request()->validate([
            'username' => 'required|string',
            'password' => 'required|string',
        ]);

        return $attributes;
    }
}
