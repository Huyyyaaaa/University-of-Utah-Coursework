clear; 
clc;

%% Define the unknown function to interpolate
 f = @(x) 1./(1+25*x.^2);

%% Setup different numbers of interpolation nodes/data sites.
Nd = [2,4,8,16,32,64,128,256];
Nd2 = [9,50];

%% Setup the points at which we want to evaluate the interpolant
xe = linspace(-1,1,1000)'; %generate evenly-spaced points
ye = f(xe); %evaluate function

%% Preallocate space for numerical solution
ye_num = zeros(length(ye),length(Nd));
ye_cheb = zeros(length(ye),length(Nd));
%% Start plotting stuff
% figure
% plot(xe,ye,'black');

figure

pause %waits for input

%% Loop goes over different numbers of interpolation nodes
for it=1:length(Nd)
   x = linspace(-1,1,Nd(it))'; %generate evenly-spaced points in [-1,1].
   x = chebextrema(-1,1,Nd(it)); %generate points using chebyshev extrema.
    y = f(x);
    
    %% Form Vandermonde matrix "intelligently"
    V = zeros(length(x),length(x));
    for vit = 1:Nd(it)
        V(:,vit) = x.^(vit-1);
    end
    
    %% Compute coefficients by solving linear system
    tic
    a = V\y;
    
    %% Evaluate polynomial interpolant by building eval matrix and
    %% multiplying with coefficients.
    Ve = zeros(length(xe),length(x));
    for vit = 1:Nd(it)
        Ve(:,vit) = xe.^(vit-1);
    end
    ye_num(:,it) = Ve*a; %Do the matrix-vector product to predict values
    elapsed = toc;
    %% Plot condition number vs number nodes
%     hold on;
%     plot(Nd(:,it),cond(ye_num(:,it)),'o');
%     drawnow
%     pause
    
    %plot for polynomial interpolation
%     hold on;
%     plot(xe,ye_num(:,it),'o');
%     drawnow
%     pause
%   plot for time vs accuracy
    hold on;
    plot(elapsed, Nd(:,it), Nd(:,it),(norm(ye_num(:,it)-ye)./norm(ye)),'o');
    axis([0 300 -5 10]);
    drawnow
    pause    
    
    
end

%% Now plot relative errors
figure
e2 = zeros(length(Nd),1);
for it=1:length(Nd)
    e2(it,1) = norm(ye_num(:,it)-ye)./norm(ye);
end
semilogy(Nd,e2);
    