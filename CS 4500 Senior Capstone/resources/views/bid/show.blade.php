@extends('spark::layouts.app')

@section('content')
    <bid-show :bid="{{ json_encode($bid) }}"></bid-show>
@endsection
