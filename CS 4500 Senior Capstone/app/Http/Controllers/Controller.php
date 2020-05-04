<?php

namespace App\Http\Controllers;

use Illuminate\Foundation\Auth\Access\AuthorizesRequests;
use Illuminate\Foundation\Bus\DispatchesJobs;
use Illuminate\Foundation\Validation\ValidatesRequests;
use Illuminate\Routing\Controller as BaseController;

class Controller extends BaseController
{
    use AuthorizesRequests, DispatchesJobs, ValidatesRequests;

    public function success($message, $options = [])
    {
        return array_merge([
            'status' => 'success',
            'message' => $message,
        ], $options);
    }

    public function warning($message)
    {
        return [
            'status' => 'warning',
            'message' => $message
        ];
    }

    public function error($message)
    {
        return [
            'status' => 'error',
            'message' => $message
        ];
    }
}
