clear;
clc;

%% Define function to interpolate
 f = @(x) 1./(1+25*x.^2);

 %setup different number of points
 Nd = [2,4,8,16,32,64,128,256];
%  Nd = [9,50]; % assignment 1(c) points
 
% Setup the points at which we want to evaluate the interpolant
xe = linspace(-1,1,10000)'; %generate evenly-spaced points
ye = f(xe); %evaluate function

% Preallocate space for numerical solution
ye_num = zeros(length(ye),length(Nd));

%% Loop for equispaced points interpolation
figure
plot(xe,ye,'black');
pause

for it=1:length(Nd)
   x = linspace(-1,1,Nd(it))'; %generate evenly-spaced points in [-1,1].
    y = f(x);
    
    % Form Vandermonde matrix "intelligently"
    V = zeros(length(x),length(x));
    for vit = 1:Nd(it)
        V(:,vit) = x.^(vit-1);
    end
    
    a = V\y;
    
    % Evaluate polynomial interpolant by building eval matrix and
    % multiplying with coefficients.
    Ve = zeros(length(xe),length(x));
    for vit = 1:Nd(it)
        Ve(:,vit) = xe.^(vit-1);
    end
    ye_num(:,it) = Ve*a; %Do the matrix-vector product to predict values
   

% Plots for equispaced points on Vandermonde

    hold on;
    plot(xe,ye_num(:,it),'o');
    drawnow
    pause
    
end %loop end

%% Plots for equispaced
figure
pause
for it=1:length(Nd)
    hold on;
    plot(Nd(it),cond(ye_num(:,it)),'o');
    drawnow
    pause
end

%% Loop for chebyshev points interpolation
figure
plot(xe,ye,'black');
pause

elapsed = zeros(length(Nd),1);

for it=1:length(Nd)
   x = chebextrema(-1,1,Nd(it)); %generate evenly-spaced points in [-1,1].
    y = f(x);
    
    % Form Vandermonde matrix "intelligently"
    V = zeros(length(x),length(x));
    for vit = 1:Nd(it)
        V(:,vit) = x.^(vit-1);
    end
    tic %timer
    a = V\y;
    
    % Evaluate polynomial interpolant by building eval matrix and
    % multiplying with coefficients.
    Ve = zeros(length(xe),length(x));
    for vit = 1:Nd(it)
        Ve(:,vit) = xe.^(vit-1);
    end
    ye_num(:,it) = Ve*a; %Do the matrix-vector product to predict values
    elapsed(it) = toc;

% Plots for chebyshev points on Vandermonde

    hold on;
    plot(xe,ye_num(:,it),'o');
    drawnow
    pause
    
end %loop end

%% Plots for chebyshev points
figure
pause
for it=1:length(Nd)
    hold on;
    plot(Nd(it),cond(ye_num(:,it)),'o');
    drawnow
    pause
end
%time plot vs accuracy
figure
e2 = zeros(length(Nd),1);
for it=1:length(Nd)
     e2(it,1) = norm(ye_num(:,it)-ye)./norm(ye);
end
semilogy(elapsed,e2);
