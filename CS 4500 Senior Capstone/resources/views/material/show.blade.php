@extends('spark::layouts.app')

@section('content')
    <material-show :material="{{ json_encode($material) }}"></material-show>
@endsection
