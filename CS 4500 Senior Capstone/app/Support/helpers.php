<?php

use Illuminate\Support\Carbon;
use Illuminate\Support\Str;

function user(): ?App\Models\User
{
    return auth()->user();
}

function team(): ?App\Models\Team
{
    return optional(user())->current_team;
}

function carbon(...$args): Carbon
{
    return new Carbon(...$args);
}

function filter($filter)
{
    return session()->get('filters.' . $filter);
}

function isActiveIfContains($value)
{
    $currentRoute = request()->route()->getName();
    if (Str::contains($currentRoute, $value)) {
        return 'active';
    }
    return '';
}

if (!function_exists('int_to_money')) {
    function int_to_money($value, $symbol = '')
    {
        if ($value == null) return $value;

        $value = $value > 0 ? $value / 100 : 0;

        return $symbol . number_format($value, 2, '.', ',');
    }
}

if (!function_exists('int_to_decimal')) {
    function int_to_decimal($value, $symbol = '')
    {
        if ($value == null) return $value;

        $value = $value > 0 ? $value / 100 : 0;

        return $symbol . number_format($value, 2, '.', '');
    }
}


if (!function_exists('logger')) {
    /**
     * Log a debug message to the logs.
     *
     * @param string $message
     * @param array $context
     * @return \Illuminate\Log\LogManager|null
     */
    function logger($message = null, array $context = [])
    {
        if (is_null($message)) {
            return app('log');
        }

        return app('log')->debug($message, $context);
    }
}
