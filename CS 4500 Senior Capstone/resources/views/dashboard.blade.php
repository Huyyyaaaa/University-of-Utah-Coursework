@extends('spark::layouts.app')

@section('content')
    <dashboard :tasks="{{ json_encode($tasks) }}"></dashboard>
@endsection
