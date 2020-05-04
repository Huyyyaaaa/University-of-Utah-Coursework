<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;

class ContactInformationController extends Controller
{
    public function index()
    {
        return user();
    }

    public function update()
    {
        $attributes = $this->validateRequest();

        user()->update($attributes);

        return $this->success('User Information Successfully Updated!');
    }

    private function validateRequest()
    {
        $attributes = request()->validate([
            'name'  => 'required',
            'email' => 'required|email',
        ]);

        return $attributes;
    }
}
